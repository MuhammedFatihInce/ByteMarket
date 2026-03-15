using ByteMarket.WebUI.Models.Payment;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IPaymentService
	{
		Task<ApiDataResponse<GatewayResponseViewModel>> ProcessPayment(PaymentRequestViewModel request);
	}
}
