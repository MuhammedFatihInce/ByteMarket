
using ByteMarket.WebUI.Models.Currency;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class CurrencyService:ICurrencyService
	{
		private readonly IApiService _apiService;
		public CurrencyService(IApiService apiService) => _apiService = apiService;


		public async Task<ApiDataResponse<List<CurrencyViewModel>>> GetAll()
		{
			return await _apiService.GetAllAsync<CurrencyViewModel>("Currency");
		}

		public async Task<ApiDataResponse<CurrencyViewModel>> GetbyCode(string code)
		{
			return await _apiService.GetAsync<CurrencyViewModel>($"Currency/{code}");
		}


	}
}
