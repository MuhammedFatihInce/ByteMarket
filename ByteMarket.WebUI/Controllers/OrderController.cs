using ByteMarket.WebUI.Models.Order;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IPaymentService _paymentService;
		public OrderController(IOrderService orderService, IPaymentService paymentService)
		{
			_orderService = orderService;
			_paymentService = paymentService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var result = await _orderService.GetAllOrders();

			if (result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(new List<OrderListDetailViewModel>());
		}


		[HttpGet]
		public async Task<IActionResult> Detail(string id)
		{
			var result = await _orderService.GetOrderById(id);

			if (result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(new SingleOrderViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromBody]CheckoutRequest checkoutRequest)
		{
			var isSuccess = _paymentService.ProcessPayment(checkoutRequest.PaymentModel);

			if (isSuccess)
			{
				var result = await _orderService.CreateOrder(checkoutRequest.OrderModel);

				if (result.Success)
				{
					return Json(new { success = true, message = result.Message });
				}

				return Json(new { success = false, message = result.Message });
			}

			return Json(new { success = false, message = "Ödeme yapılamadı." });
		}

		
		public IActionResult Success()
		{
			return View();
		}
	}
}
