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
			if (checkoutRequest == null || checkoutRequest.PaymentModel == null)
			{
				return Json(new { success = false, message = "Geçersiz istek. Lütfen bilgilerinizi kontrol edip tekrar deneyin." });
			}

			var paymentResult = await _paymentService.ProcessPayment(checkoutRequest.PaymentModel);

			if (paymentResult != null && paymentResult.Data != null && checkoutRequest.OrderModel != null && paymentResult.Success)
			{
				var orderDataJson = JsonSerializer.Serialize(checkoutRequest.OrderModel);

				HttpContext.Session.SetString("TransactionToken", paymentResult.Data.TransactionToken);
				HttpContext.Session.SetString("PendingOrder", orderDataJson);

				return Json(new { success = true,
					redirectUrl = paymentResult.Data.RedirectUrl
				});
			}

			string errorMessage = paymentResult != null && !string.IsNullOrEmpty(paymentResult.Message)
				? paymentResult.Message
				: "Ödeme servisi yanıt vermedi veya işlem reddedildi.";

			return Json(new { success = false, message = "Ödeme başlatılamadı: " + errorMessage });
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCallback(string token)
		{
			var transactionToken = HttpContext.Session.GetString("TransactionToken");

			if (!string.IsNullOrEmpty(token) && transactionToken == token)
			{

				var verificationResult = await _paymentService.VerifyPayment(token);

				if (verificationResult != null && verificationResult.Success)
				{
					var orderJson = HttpContext.Session.GetString("PendingOrder");

					if (!string.IsNullOrEmpty(orderJson))
					{
						var orderModel = JsonSerializer.Deserialize<CreateOrderViewModel>(orderJson);

						
						var result = await _orderService.CreateOrder(orderModel);

						if (result.Success)
						{
							
							HttpContext.Session.Remove("TransactionToken");
							HttpContext.Session.Remove("PendingOrder");
							return RedirectToAction("Success", new { id = orderModel.BasketId });
						}
						else
						{
							TempData["ErrorMessage"] = "Ödeme alındı ancak sipariş oluşturulurken hata çıktı: " + result.Message;
						}
					}
				}
				else
				{
					TempData["ErrorMessage"] = "Ödeme doğrulanamadı! Sahte işlem şüphesi.";
				}

				
			}
			else
			{
				TempData["ErrorMessage"] = "Geçersiz işlem oturumu.";
			}

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
