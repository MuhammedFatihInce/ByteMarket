
namespace ByteMarket.Business.Utilities.Results
{
	public interface IDataResult<T> : IResult
	{
		T Data { get; }
	}
}
