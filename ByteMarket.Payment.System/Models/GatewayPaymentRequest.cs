namespace ByteMarket.Payment.System.Models
{
	public class GatewayPaymentRequest
	{
		public PaymentRequest PaymentRequest { get; set; }
		public string ReturnUrl { get; set; } 
	}
}
