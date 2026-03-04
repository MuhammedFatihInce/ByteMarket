
using ByteMarket.Business.DTOs.Order;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IOrderService
	{
		Task<IResult> CreateOrderAsync(CreateOrderDto createOrderDto);
		Task<IDataResult<List<OrderListDetailDto>>> GetAllOrdersAsync();
		Task<IDataResult<SingleOrderDto>> GetOrderByIdAsync(string id);
	}
}
