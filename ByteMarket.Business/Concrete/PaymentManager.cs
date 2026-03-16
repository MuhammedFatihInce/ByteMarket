
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Payment;
using ByteMarket.Business.Utilities.Results;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace ByteMarket.Business.Concrete
{
	public class PaymentManager: IPaymentService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public PaymentManager(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}


		public async Task<IDataResult<GatewayResponse>> InitializePaymentAsync(PaymentRequest request)
		{
			var gatewayData = new
			{
				PaymentRequest = request,
				ReturnUrl = _configuration["PaymentSystemSettings:Return_Url"],
			};

			var response = await _httpClient.PostAsJsonAsync(_configuration["PaymentSystemSettings:Payment_Url"], gatewayData);
			var responseDto = await response.Content.ReadFromJsonAsync<GatewayResponse>();

			if (responseDto.IsSuccess)
			{
				return new SuccessDataResult<GatewayResponse>(responseDto);
			}

			return new ErrorDataResult<GatewayResponse>(responseDto.Message);

		}

		public async Task<IDataResult<GatewayResponse>> VerifyPaymentAsync(string token)
		{
			
			string verifyUrl = _configuration["PaymentSystemSettings:Verify_Url"];

			var response = await _httpClient.PostAsJsonAsync(verifyUrl, token);
			var responseDto = await response.Content.ReadFromJsonAsync<GatewayResponse>();

			if (responseDto != null && responseDto.IsSuccess)
			{
				return new SuccessDataResult<GatewayResponse>(responseDto);
			}

			return new ErrorDataResult<GatewayResponse>("Ödeme doğrulanamadı.");
		}
	}
}
