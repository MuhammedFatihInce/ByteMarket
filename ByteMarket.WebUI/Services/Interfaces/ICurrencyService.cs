using ByteMarket.WebUI.Models.Currency;
using ByteMarket.WebUI.Models.ResultModels;


namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface ICurrencyService
	{
		Task<ApiDataResponse<List<CurrencyViewModel>>> GetAll();
		Task<ApiDataResponse<CurrencyViewModel>> GetbyCode(string code);

	}
}
