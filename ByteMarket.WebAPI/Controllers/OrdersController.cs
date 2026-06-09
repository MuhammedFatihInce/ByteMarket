using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.WebAPI.Hubs;
using ByteMarket.WebAPI.SignalRServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize]
	public class OrdersController : BaseController
	{
		readonly IOrderService _orderService;
		readonly IStockNotificationService _stockNotificationService;

		public OrdersController(IOrderService orderService, IStockNotificationService stockNotificationService)
		{
			_orderService = orderService;
			_stockNotificationService = stockNotificationService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
		{
			var result = await _orderService.CreateOrderAsync(createOrderDto);

			if (result.Success && result.Data != null)
			{
				foreach (var stockInfo in result.Data)
				{
					await _stockNotificationService.SendStockUpdateAsync(stockInfo.ProductId, stockInfo.NewStock);
				}
			}

			var response = new Result(result.Success, result.Message);
			
			return CreateActionResult(response);
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
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var result = await _orderService.GetOrderByIdAsync(id, userId);
			return CreateActionResult(result, errorStatusCode:404);
		}

		[HttpPost("SendInvoice/{id}")]
		public async Task<IActionResult> SendInvoice(string id)
		{
			var result = await _orderService.SendInvoiceMassegeAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpGet("Invoice/{id}")]
		public async Task<IActionResult> GetInvoiceById(string id)
		{
			var result = await _orderService.GetInvoiceOrderByIdAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
