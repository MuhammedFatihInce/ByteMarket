using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IEditorService
	{
		Task<ApiDataResponse<string>> Upload(IFormFile file);
		Task<ApiDataResponse<string>> Delete(string url);
	}
}
