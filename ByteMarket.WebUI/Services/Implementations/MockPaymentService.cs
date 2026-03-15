using ByteMarket.WebUI.Models.Payment;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class MockPaymentService: IPaymentService
	{
		private readonly IApiService _apiService;

		public MockPaymentService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<GatewayResponseViewModel>> ProcessPayment(PaymentRequestViewModel request)
		{
			return await _apiService.PostAsync<GatewayResponseViewModel>("Payments/initialize", request);
		}
	}
}
