
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Basket;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Abstract.BasketItem;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.Entities.Concrete;
using ByteMarket.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ByteMarket.DataAccess.Abstract.Coupon;
using ByteMarket.DataAccess.Abstract.Order;
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
		readonly ICouponReadRepository _couponReadRepository;
		readonly IOrderReadRepository _orderReadRepository;
		public BasketManager(IHttpContextAccessor httpContextAccessor, IBasketReadRepository basketReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IProductReadRepository productReadRepository, ICouponReadRepository couponReadRepository, IOrderReadRepository orderReadRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_basketWriteRepository = basketWriteRepository;
			_basketReadRepository = basketReadRepository;
			_basketItemReadRepository = basketItemReadRepository;
			_basketItemWriteRepository = basketItemWriteRepository;
			_productReadRepository = productReadRepository;
			_couponReadRepository = couponReadRepository;
			_orderReadRepository = orderReadRepository;
		}

		private async Task<Basket> GetActiveBasketOfUser()
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrWhiteSpace(currentUserId))
			{
				
				var activeBasket =
					await _basketReadRepository.Table
						.Include(b=>b.Order)
						.Include(b=> b.Coupons)
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

		public async Task<IDataResult<ListBasketDto>> GetBasketItemsAsync()
		{
			var basket = await GetActiveBasketOfUser();

			Basket? result = await _basketReadRepository.Table
				.Include(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.ThenInclude(p=>p.ProductImageFiles)
				.Include(b => b.Coupons).ThenInclude(c=>c.Products)
				.FirstOrDefaultAsync(b => b.Id == basket.Id);

			if (result == null)
				return new ErrorDataResult<ListBasketDto>("Sepet bulunamadı.");

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
								: null,
					DiscountAmount = 0
			}).ToList();


			decimal discountAmount = 0;
			if (result.Coupons != null && result.Coupons.Any())
			{
				foreach (var coupon in result.Coupons)
				{
					discountAmount += CalculateDiscount(basketItemsDto, coupon);
				}
				
			}



			decimal rawTotal = basketItemsDto.Sum(x => x.Total);

			var basketDto = new ListBasketDto
			{
				Id = result.Id.ToString(),
				BasketItem = basketItemsDto,
				TotalBasePrice = rawTotal,                   
				DiscountAmount = discountAmount,              
				FinalTotalPrice = rawTotal - discountAmount,
				Coupons = result.Coupons?.Select(c=>new ApplyCouponDto
				{
					Id = c.Id.ToString(),
					Name = c.Name,
					Code = c.Code
				}).ToList()
			};

			 return new SuccessDataResult<ListBasketDto>(basketDto);
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

		private decimal CalculateDiscount(List<BasketItemDto> basketItems, Coupon coupon)
		{
			if (coupon == null || basketItems == null) return 0;

			decimal totalDiscount = 0;

			if (coupon.Target == DiscountTarget.AllOrder)
			{
				decimal totalPrice = basketItems.Sum(x => x.Price * x.Quantity);
				totalDiscount = coupon.IsPercentage
					? (totalPrice * coupon.DiscountValue / 100)
					: coupon.DiscountValue;
			}
			else if (coupon.Target == DiscountTarget.SpecificProducts)
			{
				if (coupon.Products == null) return 0;

				var validProductIds = coupon.Products.Select(p => p.Id.ToString()).ToHashSet();

				var eligibleItems = basketItems.Where(item =>
					validProductIds.Contains(item.ProductId));

				foreach (var item in eligibleItems)
				{
					decimal itemDiscount = 0;

					if (coupon.IsPercentage)
					{
						itemDiscount += (item.Total * coupon.DiscountValue / 100);
					}
					else
					{
						itemDiscount += (coupon.DiscountValue * item.Quantity);
					}

					item.DiscountAmount += itemDiscount;
					totalDiscount += itemDiscount;
				}
			}

			return totalDiscount;
		}
		public async Task<IResult> ApplyCouponToBasketAsync(string couponCode)
		{

			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			var basket = await GetActiveBasketOfUser();

			var coupon = await _couponReadRepository.GetSingleAsync(c => c.Code == couponCode && c.ExpireTime >= DateTime.UtcNow);

			if (coupon == null)
				return new ErrorResult("Geçersiz veya süresi dolmuş kupon.");


			if (coupon.UsageLimitPerUser > 0)
			{
				
				var usedCount = await _orderReadRepository.Table
					.CountAsync(o => o.Basket.UserId == Guid.Parse(currentUserId) &&
					                 o.Basket.Coupons.Any(c => c.Id == coupon.Id));

				if (usedCount >= coupon.UsageLimitPerUser)
				{
					return new ErrorResult($"Bu kuponu en fazla {coupon.UsageLimitPerUser} kez kullanabilirsiniz. Kullanım limitiniz dolmuştur.");
				}
			}

			basket.Coupons ??= new List<Coupon>();

			if (basket.Coupons.Any(c => c.Code == couponCode))
				return new ErrorResult("Bu kupon zaten uygulanmış.");

			if (basket.Coupons.Any())
			{
				if (!coupon.IsStackable)
					return new ErrorResult("Bu kupon diğer kuponlarla birleştirilemez.");

				if (basket.Coupons.Any(existingCoupon => !existingCoupon.IsStackable))
					return new ErrorResult("Sepetinizde birleştirilemez bir kupon bulunduğu için yeni kupon eklenemez.");
			}

			basket.Coupons.Add(coupon);
			await _basketWriteRepository.SaveAsync();

			return new SuccessResult("Kupon başarıyla uygulandı.");

		}

		public async Task<IResult> RemoveCouponFromBasketAsync(string id)
		{
			if (!Guid.TryParse(id, out Guid couponId))
				return new ErrorResult("Geçersiz kupon formatı.");

			var basket = await GetActiveBasketOfUser();

			var coupon = await _couponReadRepository.GetSingleAsync(c => c.Id == couponId);

			if (coupon == null)
				return new ErrorResult("Kupon sistemde bulunamadı.");


			if (!basket.Coupons.Any(c => c.Id == couponId))
				return new ErrorResult("Bu kupon zaten bu sepete ait değil.");

			var couponToRemove = basket.Coupons.FirstOrDefault(c => c.Id == couponId);

			if (couponToRemove != null)
			{
				basket.Coupons.Remove(couponToRemove);
			}

			await _basketWriteRepository.SaveAsync();

			return new SuccessResult("Kupon başarıyla kaldırıldı.");
		}

	}
}
