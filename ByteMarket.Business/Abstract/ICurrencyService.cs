
using ByteMarket.Business.DTOs.Currency;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface ICurrencyService
	{
		Task UpdateDatabaseCurrencies();

		Task<IDataResult<List<CurrencyDto>>> GetAllCurrenciesFromDbAsync();
		Task<IDataResult<CurrencyDto>> GetCurrencyByCodeAsync(string code);
	}
}
