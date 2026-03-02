using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Models.wishList;
using ByteMarket.WebUI.Services.Interfaces;


namespace ByteMarket.WebUI.Services.Implementations
{
	public class WishListService: IWishListService
	{
		private readonly IApiService _apiService;

		public WishListService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<List<WishListProductViewModel>>> GetAllWishListProductsAsync()
		{
			return await _apiService.GetAllAsync<WishListProductViewModel>("WishList");
		}

		public async Task<ApiDataResponse<object>> AddWishListProductAsync(string productId)
		{
			return await _apiService.PostAsync<object>("WishList", productId);
		}

		public async Task<ApiDataResponse<object>> DeleteWishListAsync(string productId)
		{
			return await _apiService.DeleteAsync<object>("WishList", productId);
		}
	}
}
