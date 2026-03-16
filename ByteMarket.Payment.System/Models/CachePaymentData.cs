namespace ByteMarket.Payment.System.Models
{
	public class CachePaymentData
	{
		public string Code { get; set; }
		public string ReturnUrl { get; set; }
		public bool IsApproved { get; set; } = false;
	}
}
