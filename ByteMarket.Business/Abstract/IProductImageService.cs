using Microsoft.AspNetCore.Http;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface IProductImageService
	{
		Task<IResult> AddImagesAsync(string productId, IFormFileCollection files);
		Task<IResult> DeleteImageAsync(string imageId);
		Task<IResult> DeleteImagesByProductIdAsync(string productId);
	}
}
