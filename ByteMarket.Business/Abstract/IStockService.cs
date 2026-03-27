
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IStockService
	{
		Task<IResult> CheckStockAsync(string basketId);

		Task<IResult> CheckAndDecreaseStockAsync(string basketId);
	}
}
