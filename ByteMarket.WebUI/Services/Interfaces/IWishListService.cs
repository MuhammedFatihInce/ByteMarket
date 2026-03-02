using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Models.wishList;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IWishListService
	{
		Task<ApiDataResponse<List<WishListProductViewModel>>> GetAllWishListProductsAsync();
		Task<ApiDataResponse<object>> AddWishListProductAsync(string productId);
		Task<ApiDataResponse<object>> DeleteWishListAsync(string id);
	}
}
