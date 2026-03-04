using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize]
	public class OrdersController : BaseController
	{
		readonly IOrderService _orderService;

		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
		{
			var result = await _orderService.CreateOrderAsync(createOrderDto);
			return CreateActionResult(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOrders()
		{
			var result = await _orderService.GetAllOrdersAsync();
			return CreateActionResult(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderById(string id)
		{
			var result = await _orderService.GetOrderByIdAsync(id);
			return CreateActionResult(result, errorStatusCode:404);
		}

	}
}
