
using ByteMarket.Business.DTOs.Payment;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IPaymentService
	{
		Task<IDataResult<GatewayResponse>> InitializePaymentAsync(PaymentRequest request, string basketId);
		Task<IDataResult<GatewayResponse>> VerifyPaymentAsync(string token);
	}
}
