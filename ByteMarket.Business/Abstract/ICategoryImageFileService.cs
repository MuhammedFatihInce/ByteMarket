
using Microsoft.AspNetCore.Http;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface ICategoryImageFileService
	{
		Task<IResult> AddImageAsync(string categoryId, IFormFile file);
		Task<IResult> DeleteImageAsync(string imageId);
		Task<IResult> DeleteImageByCategoryIdAsync(string categoryId);
	}
}
