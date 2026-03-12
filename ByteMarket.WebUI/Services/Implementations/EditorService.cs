using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class EditorService:IEditorService
	{
		private readonly IApiService _apiService;

		public EditorService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<string>> Upload(IFormFile file)
		{
			
			using var content = new MultipartFormDataContent();
			var fileContent = new StreamContent(file.OpenReadStream());

			fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

			content.Add(fileContent, "file", file.FileName);

			return await _apiService.PostMultipartAsync<string>("Editor", content);
		}


		public async Task<ApiDataResponse<string>> Delete(string url)
		{

			string encodedUrl = WebUtility.UrlEncode(url);
			string endpoint = $"Editor?url={encodedUrl}";

			return await _apiService.DeleteAsync<string>(endpoint);
		}

	}
}
