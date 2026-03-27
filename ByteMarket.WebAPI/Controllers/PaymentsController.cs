using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class PaymentsController : BaseController
	{
		private readonly IPaymentService _paymentService;

		public PaymentsController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpPost("initialize")]
		public async Task<IActionResult> Initialize([FromBody] PaymentRequest request, [FromQuery]string basketId)
		{
			var result = await _paymentService.InitializePaymentAsync(request, basketId);

			return CreateActionResult(result);
		}

		[HttpPost("verify")]
		public async Task<IActionResult> Verify([FromBody] string token)
		{
			var result = await _paymentService.VerifyPaymentAsync(token);
			return CreateActionResult(result);
		}

	}
}
