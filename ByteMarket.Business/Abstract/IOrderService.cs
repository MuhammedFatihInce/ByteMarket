
using ByteMarket.Business.DTOs.Order;
using ByteMarket.Business.DTOs.Stock;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IOrderService
	{
		Task<IDataResult<List<StockUpdateDto>>> CreateOrderAsync(CreateOrderDto createOrderDto);
		Task<IDataResult<List<OrderListDetailDto>>> GetAllOrdersAsync();
		Task<IDataResult<SingleOrderDto>> GetOrderByIdAsync(string id);
		Task<IResult> SendInvoiceMassegeAsync(string id);
	}
}
