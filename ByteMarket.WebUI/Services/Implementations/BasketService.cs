using ByteMarket.WebUI.Models.Basket;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class BasketService: IBasketService
	{
		private readonly IApiService _apiService;

		public BasketService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<object>> AddItemToBasket(CreateBasketViewModel model)
		{
			return await _apiService.PostAsync<object>("Baskets", model);
		}


		public async Task<ApiDataResponse<ListBasketViewModel>> GetBasketItems()
		{
			return await _apiService.GetAsync<ListBasketViewModel>("Baskets");
		}

		public async Task<ApiDataResponse<object>> RemoveBasketItem(string basketItemId)
		{
			return await _apiService.DeleteAsync<object>("Baskets", basketItemId);
		}

		public async Task<ApiDataResponse<object>> UpdateQuantity(UpdateBasketItemQuantityViewModel model)
		{
			return await _apiService.PutAsync<object>("Baskets", model);
		}

		public async Task<ApiDataResponse<object>> ApplyCouponToBasketAsync(string couponCode)
		{
			return await _apiService.PostAsync<object>("Baskets/ApplyCouponToBasket", couponCode);
		}

		public async Task<ApiDataResponse<object>> RemoveCouponFromBasket(string couponId)
		{
			return await _apiService.DeleteAsync<object>("Baskets/RemoveCouponFromBasket", couponId);
		}

	}
}
