
using ByteMarket.Business.Utilities.Results;
using Microsoft.AspNetCore.Http;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface IEditorService
	{
		Task<IDataResult<string>> UploadImageAsync(IFormFile file, string folderName);
		Task<IResult> DeleteImageAsync(string fileUrl, string folderName);
	}
}
