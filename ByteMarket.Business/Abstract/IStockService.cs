
using ByteMarket.Business.DTOs.Stock;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IStockService
	{
		Task<IResult> CheckStockAsync(string basketId);

		Task<IDataResult<List<StockUpdateDto>>> CheckAndDecreaseStockAsync(string basketId);
	}
}
