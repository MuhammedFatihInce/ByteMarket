using ByteMarket.WebUI.Models.Order;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IOrderService
	{
		Task<ApiDataResponse<object>> CreateOrder(CreateOrderViewModel model);
		Task<ApiDataResponse<List<OrderListDetailViewModel>>> GetAllOrders();
		Task<ApiDataResponse<SingleOrderViewModel>> GetOrderById(string id);
		Task<ApiDataResponse<object>> SendInvoice(string id);
		Task<ApiDataResponse<InvoiceOrderViewmodel>> GetInvoiceOrderByIdAsync(string id);
	}
}
