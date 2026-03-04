using ByteMarket.WebUI.Models.Payment;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class MockPaymentService: IPaymentService
	{
		public bool ProcessPayment(PaymentRequestViewModel request)
		{
			if (request.CardNumber.StartsWith("1111"))
			{
				return true; 
			}

			return false;
		}
	}
}
