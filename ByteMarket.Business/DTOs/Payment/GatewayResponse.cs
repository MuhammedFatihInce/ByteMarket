
namespace ByteMarket.Business.DTOs.Payment
{
	public class GatewayResponse
	{
		public bool IsSuccess { get; set; }
		public string RedirectUrl { get; set; }
		public string TransactionToken { get; set; }
		public string Message { get; set; }

	}
}
