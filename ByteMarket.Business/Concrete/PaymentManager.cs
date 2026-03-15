
using System.Net.Http.Json;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Payment;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Concrete
{
	public class PaymentManager: IPaymentService
	{
		private readonly HttpClient _httpClient;

		public PaymentManager(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		public async Task<IDataResult<GatewayResponse>> InitializePaymentAsync(PaymentRequest request)
		{
			var gatewayData = new
			{
				PaymentRequest = request,
				ReturnUrl = "https://localhost:44380/Order/PaymentCallback",
			};

			var response = await _httpClient.PostAsJsonAsync("https://localhost:44338/Payment/InitPayment", gatewayData);
			var responseDto = await response.Content.ReadFromJsonAsync<GatewayResponse>();

			if (responseDto.IsSuccess)
			{
				return new SuccessDataResult<GatewayResponse>(responseDto);
			}

			return new ErrorDataResult<GatewayResponse>(responseDto.Message);

		}
	}
}
