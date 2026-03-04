using ByteMarket.WebUI.Models.Basket;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IBasketService
	{
		Task<ApiDataResponse<object>> AddItemToBasket(CreateBasketViewModel model);
		Task<ApiDataResponse<List<BasketItemViewModel>>> GetBasketItems();
		Task<ApiDataResponse<object>> RemoveBasketItem(string basketItemId);
		Task<ApiDataResponse<object>> UpdateQuantity(UpdateBasketItemQuantityViewModel model);
	}
}
