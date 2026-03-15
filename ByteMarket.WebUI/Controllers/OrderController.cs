using ByteMarket.WebUI.Models.Order;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


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
			var paymentResult = await _paymentService.ProcessPayment(checkoutRequest.PaymentModel);

			if (paymentResult.Success && paymentResult.Data != null && checkoutRequest.OrderModel != null)
			{
				var orderDataJson = JsonSerializer.Serialize(checkoutRequest.OrderModel);

				HttpContext.Session.SetString("TransactionToken", paymentResult.Data.TransactionToken);
				HttpContext.Session.SetString("PendingOrder", orderDataJson);

				return Json(new { success = true,
					redirectUrl = paymentResult.Data.RedirectUrl
				});
			}

			return Json(new { success = false, message = "Ödeme başlatılamadı: " + paymentResult.Message });
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCallback(string token, bool success)
		{
			var transactionToken = HttpContext.Session.GetString("TransactionToken");

			if (success && transactionToken == token)
			{
				var orderJson = HttpContext.Session.GetString("PendingOrder");

				if (!string.IsNullOrEmpty(orderJson))
				{
					var orderModel = JsonSerializer.Deserialize<CreateOrderViewModel>(orderJson); // Kendi modeline göre cast et

					// 2. Ödeme başarılı olduğuna göre artık SİPARİŞİ OLUŞTURABİLİRİZ
					var result = await _orderService.CreateOrder(orderModel);

					if (result.Success)
					{
						// Session'ı temizle
						HttpContext.Session.Remove("TransactionToken");
						HttpContext.Session.Remove("PendingOrder");
						return RedirectToAction("Success", new { id = orderModel.BasketId }); // "Tebrikler" sayfası
					}
				}
			}

			TempData["Error"] = "Ödeme onaylandı ancak sipariş oluşturulurken bir hata oluştu.";
			return RedirectToAction("Index", "Basket");
		}



		public async Task<IActionResult> Success(string id)
		{
			var result = await _orderService.GetOrderById(id);

			if (result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(new SingleOrderViewModel());
		}


		[HttpPost("/Order/SendInvoice/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendInvoice(string id)
		{
			var result = await _orderService.SendInvoice(id);

			return Json(new { success = result.Success, message = result.Message });
		}

		

	}
}
