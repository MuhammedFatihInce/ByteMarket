namespace ByteMarket.WebUI.Models.Payment
{
	public class GatewayResponseViewModel
	{
		public bool IsSuccess { get; set; }
		public string RedirectUrl { get; set; }
		public string TransactionToken { get; set; }
		public string Message { get; set; }

	}
}
