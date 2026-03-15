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
		public async Task<IActionResult> Initialize([FromBody] PaymentRequest request)
		{
			var result = await _paymentService.InitializePaymentAsync(request);

			return CreateActionResult(result);
		}

	}
}
