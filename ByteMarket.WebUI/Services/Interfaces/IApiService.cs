using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IApiService
	{
		Task<ApiDataResponse<List<T>>> GetAllAsync<T>(string endpoint);
		Task<ApiDataResponse<T>> GetByIdAsync<T>(string endpoint, string id); 
		Task<ApiDataResponse<T>> PostAsync<T>(string endpoint, object data);
		Task<ApiDataResponse<T>> PutAsync<T>(string endpoint, object data);
		Task<ApiDataResponse<bool>> DeleteAsync(string endpoint, string id);
		Task<ApiDataResponse<T>> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent content);
	}
}
