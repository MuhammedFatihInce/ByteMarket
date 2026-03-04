using ByteMarket.WebUI.Models.Basket;
using ByteMarket.WebUI.Models.Order;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class OrderService: IOrderService
	{
		private readonly IApiService _apiService;

		public OrderService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<object>> CreateOrder(CreateOrderViewModel model)
		{
			return await _apiService.PostAsync<object>("Orders", model);
		}


		public async Task<ApiDataResponse<List<OrderListDetailViewModel>>> GetAllOrders()
		{
			return await _apiService.GetAllAsync<OrderListDetailViewModel>("Orders");
		}

		public async Task<ApiDataResponse<SingleOrderViewModel>> GetOrderById(string id)
		{
			return await _apiService.GetByIdAsync<SingleOrderViewModel>("Orders", id);
		}
	}
}
