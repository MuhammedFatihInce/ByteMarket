
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.Utilities.Results;
using Microsoft.AspNetCore.Http;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class EditorManager : IEditorService
	{
		private readonly IStorageService _storageService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public EditorManager(IStorageService storageService, IHttpContextAccessor httpContextAccessor)
		{
			_storageService = storageService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IDataResult<string>> UploadImageAsync(IFormFile file, string folderName)
		{
			
			IFormFileCollection files = new FormFileCollection { file };

			var result = await _storageService.UploadAsync(folderName, files);

			var uploadedFilePath = result.FirstOrDefault().pathOrContainerName;

			string webPath = "/" + uploadedFilePath.Replace("\\", "/");

			var request = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{request.Scheme}://{request.Host}";

			string fullPath = $"{baseUrl}/{webPath}";

			return new SuccessDataResult<string>(fullPath, "Resim başarıyla yüklendi.");
		}

		public async Task<IResult> DeleteImageAsync(string fileUrl, string folderName)
		{
			if (string.IsNullOrEmpty(fileUrl))
				return new ErrorResult("Geçersiz dosya yolu.");

			string fileName = Path.GetFileName(fileUrl);

			string fullPath = Path.Combine(folderName, fileName);

			await _storageService.DeleteAsync(fullPath, fileName);

			return new SuccessResult("Resim sunucudan başarıyla silindi.");
		}
	}
}
