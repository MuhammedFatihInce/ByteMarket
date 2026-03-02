
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.WishList;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.WishList;
using ByteMarket.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class WishListManager : IWishListService
	{
		private readonly IWishListReadRepository _wishListReadRepository;
		private readonly IWishListWriteRepository _wishListWriteRepository;
		private readonly IMapper _mapper;

		public WishListManager(IWishListReadRepository wishListReadRepository, IWishListWriteRepository wishListWriteRepository, IMapper mapper)
		{
			_wishListReadRepository = wishListReadRepository;
			_wishListWriteRepository = wishListWriteRepository;
			_mapper = mapper;
		}
		public async Task<IResult> AddWishlistProductAsync(string userId, string productId)
		{
			Guid userGuid = Guid.Parse(userId);
			Guid productGuid = Guid.Parse(productId);

			var isAlreadyExists = await _wishListReadRepository
				.AnyAsync(w => w.UserId == userGuid && w.ProductId == productGuid, false);

			if (isAlreadyExists)
				return new ErrorResult("Ürün istek listesine daha önce eklenmiş.");


			var newItem = new WishList
			{
				ProductId = productGuid,
				UserId = userGuid
			};

			var result = await _wishListWriteRepository.AddAsync(newItem);

			if (result)
			{
				await _wishListWriteRepository.SaveAsync();
				return new SuccessResult("Ürün istek listesine başarıyla eklendi.");
			}

			return new ErrorResult("Ürün istek listesine eklenirken bir hata oluştu.");
		}

		public async Task<IDataResult<List<WishListProduct>>> GetAllWishListProductsAsync(string userId)
		{
			Guid userGuid = Guid.Parse(userId);

			var wishListProductsDto = await _wishListReadRepository
				.GetWhere(w => w.UserId == userGuid, false)
				.ProjectTo<WishListProduct>(_mapper.ConfigurationProvider) 
				.ToListAsync();

			if (wishListProductsDto == null || !wishListProductsDto.Any())
				return new ErrorDataResult<List<WishListProduct>>( "İstek listesine ürün bulunamadı.");

			return new SuccessDataResult<List<WishListProduct>>(wishListProductsDto,"Ürünler listelendi.");
		}

		public async Task<IResult> RemoveWishListProductAsync(string userId, string productId)
		{
			var wishListProduct = await _wishListReadRepository.GetSingleAsync(w=>w.ProductId == Guid.Parse(productId) && w.UserId == Guid.Parse(userId));
				
			if (wishListProduct == null)
				return new ErrorResult("İstek listesinde bu ürün bulunamadı.");

			var result = _wishListWriteRepository.Remove(wishListProduct);

			if (result)
			{
				
				await _wishListWriteRepository.SaveAsync();
				return new SuccessResult("Ürün istek listenizden başarıyla silindi.");
			}

			return new ErrorResult("Silme işlemi sırasında bir hata oluştu.");
		}
	}
}
