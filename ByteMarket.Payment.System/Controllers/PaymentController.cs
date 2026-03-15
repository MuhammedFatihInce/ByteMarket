using ByteMarket.Payment.System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ByteMarket.Payment.System.Controllers
{
	public class PaymentController : Controller
	{
		private readonly IMemoryCache _memoryCache;

		public PaymentController(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		
		[HttpPost]
		public IActionResult InitPayment([FromBody] GatewayPaymentRequest request)
		{
			if (!IsCardValid(request.PaymentRequest))
			{
				return Ok(new GatewayResponse
				{
					IsSuccess = false,
					Message = "Kart bilgileri yanlış."
				});
			}

			string transactionToken = Guid.NewGuid().ToString();

			
			string verificationCode = "123456"; 

			
			var cacheData = new CachePaymentData
			{
				Code = verificationCode,
				ReturnUrl = request.ReturnUrl,
			};

			_memoryCache.Set(transactionToken, cacheData, TimeSpan.FromMinutes(5));

			string redirectUrl = $"https://localhost:44338/Payment/Verify3D?token={transactionToken}";

			var response = new GatewayResponse
			{
				IsSuccess = true,
				RedirectUrl = redirectUrl,
				TransactionToken = transactionToken,
				Message = "İşlem başarılı."
			};

			return Ok(response); 
		}

		[HttpGet]
		public IActionResult Verify3D(string token)
		{
			
			if (!_memoryCache.TryGetValue(token, out CachePaymentData data))
			{
				return Content("İşlem zaman aşımına uğradı veya geçersiz token.");
			}

			ViewBag.Token = token;
			
			ViewBag.DemoCode = data.Code;

			return View();
		}

		[HttpPost]
		public IActionResult Verify3DSubmit(string token, string enteredCode)
		{
			if (_memoryCache.TryGetValue(token, out CachePaymentData data))
			{
				
				if (data.Code == enteredCode)
				{
					return Redirect($"{data.ReturnUrl}?success=true&token={token}");
				}
				else
				{
					ViewBag.Error = "Doğrulama kodu hatalı!";
					ViewBag.Token = token;
					return View("Verify3D");
				}
			}
			return Content("İşlem zaman aşımına uğradı.");
		}

		public bool IsCardValid(PaymentRequest paymentRequest)
		{
			if (paymentRequest.CardNumber != "1111111111111111")
				return false;

			if (paymentRequest.Cvv != "123")
				return false;

			if (!int.TryParse(paymentRequest.ExpirationMonth, out int month) ||
			    !int.TryParse(paymentRequest.ExpirationYear, out int year))
			{
				return false;
			}

			DateTime now = DateTime.Now;
			int currentYear = now.Year;
			int currentMonth = now.Month;

			if (month < 1 || month > 12)
				return false;

			if (year < currentYear)
				return false;

			if (year == currentYear && month < currentMonth)
				return false;

			if (string.IsNullOrWhiteSpace(paymentRequest.CardHolderName))
				return false;

			if (paymentRequest.TotalAmount > 50000)
			{
				return false;
			}

			return true;

		}
	}
}
