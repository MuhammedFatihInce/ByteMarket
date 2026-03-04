namespace ByteMarket.WebUI.Models.Payment
{
	public class PaymentRequestViewModel
	{
		public string CardNumber { get; set; }
		public string CardHolderName { get; set; }
		public string ExpirationMonth { get; set; }
		public string ExpirationYear { get; set; }
		public string Cvv { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
