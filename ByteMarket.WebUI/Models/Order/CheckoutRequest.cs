using ByteMarket.WebUI.Models.Payment;

namespace ByteMarket.WebUI.Models.Order
{
	public class CheckoutRequest
	{
		public CreateOrderViewModel OrderModel { get; set; }
		public PaymentRequestViewModel PaymentModel { get; set; }
	}
}
