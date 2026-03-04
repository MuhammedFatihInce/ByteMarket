using ByteMarket.WebUI.Models.Payment;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IPaymentService
	{
		bool ProcessPayment(PaymentRequestViewModel request);
	}
}
