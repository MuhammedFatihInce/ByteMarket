using ByteMarket.Business.DTOs.WishList;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IWishListService
	{
		Task<IResult> AddWishlistProductAsync(string userId, string productId);
		Task<IDataResult<List<WishListProduct>>> GetAllWishListProductsAsync(string userId);
		Task<IResult> RemoveWishListProductAsync(string userId, string productId);
	}
}
