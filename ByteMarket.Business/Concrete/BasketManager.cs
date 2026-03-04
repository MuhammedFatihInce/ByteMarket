
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Abstract.BasketItem;
using ByteMarket.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ByteMarket.Business.DTOs.Basket;
using ByteMarket.DataAccess.Abstract.Product;
using IResult= ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class BasketManager:IBasketService
	{
		readonly IHttpContextAccessor _httpContextAccessor;
		readonly IBasketWriteRepository _basketWriteRepository;
		readonly IBasketReadRepository _basketReadRepository;
		readonly IBasketItemReadRepository _basketItemReadRepository;
		readonly IBasketItemWriteRepository _basketItemWriteRepository;
		readonly IProductReadRepository _productReadRepository;
		public BasketManager(IHttpContextAccessor httpContextAccessor, IBasketReadRepository basketReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IProductReadRepository productReadRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_basketWriteRepository = basketWriteRepository;
			_basketReadRepository = basketReadRepository;
			_basketItemReadRepository = basketItemReadRepository;
			_basketItemWriteRepository = basketItemWriteRepository;
			_productReadRepository = productReadRepository;

		}

		private async Task<Basket> GetActiveBasketOfUser()
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrWhiteSpace(currentUserId))
			{
				
				var activeBasket =
					await _basketReadRepository.Table
						.Include(b=>b.Order)
						.FirstOrDefaultAsync(b => b.UserId == Guid.Parse(currentUserId) && b.Order == null);


				if (activeBasket == null)
				{
					activeBasket = new Basket { UserId = Guid.Parse(currentUserId) };
					await _basketWriteRepository.AddAsync(activeBasket);
					await _basketWriteRepository.SaveAsync();
				}

				return activeBasket;
			}

			throw new Exception("Beklenmeyen bir hatayla karşılaşıldı...");
		}

		public async Task<IResult> AddItemToBasketAsync(CreateBasketDto creaBasketDto)
		{
			var basket = await GetActiveBasketOfUser();

			if (basket != null)
			{
				var existingItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(creaBasketDto.ProductId));

				var product = await _productReadRepository.GetByIdAsync(creaBasketDto.ProductId);

				if (existingItem != null)
				{
					existingItem.Quantity++;
				}
				else
				{
					await _basketItemWriteRepository.AddAsync(new()
					{
						ProductId = Guid.Parse(creaBasketDto.ProductId),
						Quantity = creaBasketDto.quantity,
						BasketId = basket.Id,
						Price = product.Price
					});
				}

				await _basketWriteRepository.SaveAsync();

				return new SuccessResult("Ürün sepete başarıyla eklendi.");
			}

			return new ErrorResult("Ürün sepete eklenemedi.");
		}

		public async Task<IDataResult<List<BasketItemDto>>> GetBasketItemsAsync()
		{
			var basket = await GetActiveBasketOfUser();

			Basket? result = await _basketReadRepository.Table
				.Include(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.ThenInclude(p=>p.ProductImageFiles)
				.FirstOrDefaultAsync(b => b.Id == basket.Id);

			if (result == null)
				return new ErrorDataResult<List<BasketItemDto>>("Sepet bulunamadı.");

			var basketItemsDto = result.BasketItems.Select(bi=>new BasketItemDto
				{
					BasketItemId = bi.Id.ToString(),
					ProductId = bi.ProductId.ToString(),
					ProductName = bi.Product.Name,   
					Price = bi.Price,         
					Quantity = bi.Quantity,
					Total = bi.Price * bi.Quantity,
					ImagePath = bi.Product.ProductImageFiles.FirstOrDefault() != null
								? bi.Product.ProductImageFiles.FirstOrDefault().Path
								: null
			}).ToList();

			 return new SuccessDataResult<List<BasketItemDto>>(basketItemsDto);
		}

		public async Task<IResult> RemoveBasketItemAsync(string basketItemId)
		{
			var basket = await GetActiveBasketOfUser();

			BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);

			if (basketItem != null && basketItem.BasketId == basket.Id)
			{
				_basketItemWriteRepository.Remove(basketItem);
				await _basketItemWriteRepository.SaveAsync();
				return new SuccessResult("Ürün başarıyla sepetten silindi");
			}
			return new ErrorResult("Ürün sepetten silinemedi");
		}

		public async Task<IResult> UpdateQuantityAsync(UpdateBasketItemQuantityDto dto)
		{
			var item = await _basketItemReadRepository.GetByIdAsync(dto.BasketItemId);

			if (item != null && dto.Quantity > 0)
			{
				item.Quantity = dto.Quantity;
				await _basketWriteRepository.SaveAsync();
				return new SuccessResult();
			}

			return new ErrorResult();
		}

	}
}
