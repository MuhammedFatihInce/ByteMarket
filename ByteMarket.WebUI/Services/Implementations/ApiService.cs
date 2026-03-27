using ByteMarket.WebUI.Services.Interfaces;
using ByteMarket.WebUI.Utilities.Helpers;
using System.Text;
using System.Text.Json;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class ApiService : IApiService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly HttpClient _client;

		public ApiService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
			_client = _httpClientFactory.CreateClient("MyApiClient");
		}

		private async Task<ApiDataResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
		{
			var json = await response.Content.ReadAsStringAsync();

			//if (string.IsNullOrWhiteSpace(json))
			//{
			//	return default;
			//}
			if (json == null || json.Length == 0 && !response.IsSuccessStatusCode)
			{
				return new ApiDataResponse<T> { Success = false, Message = "Sunucu yanıt vermedi.", StatusCode = (int)response.StatusCode };
			}

			var result = JsonSerializer.Deserialize<ApiDataResponse<T>>(json, JsonOptionsHelper.Default);

			if (result != null)
			{
				result.StatusCode = (int)response.StatusCode;
				return result;
			}

			return new ApiDataResponse<T> { Success = false, Message = "Bilinmeyen Hata", StatusCode = (int)response.StatusCode };
		}

		public async Task<ApiDataResponse<List<T>>> GetAllAsync<T>(string endpoint)
		{
			var response = await _client.GetAsync(endpoint);
			return await ProcessResponse<List<T>>(response);
		}

		public async Task<ApiDataResponse<T>> GetByIdAsync<T>(string endpoint, string id)
		{
			var response = await _client.GetAsync($"{endpoint}/{id}");
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> PostAsync<T>(string endpoint, object data)
		{
			var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(endpoint, jsonContent);
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> PutAsync<T>(string endpoint, object data)
		{
			var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
			var response = await _client.PutAsync(endpoint, jsonContent);
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> DeleteAsync<T>(string endpoint, string id)
		{
			var response = await _client.DeleteAsync($"{endpoint}/{id}");
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> DeleteAsync<T>(string endpoint)
		{
			var response = await _client.DeleteAsync(endpoint);
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent content)
		{
			var response = await _client.PostAsync(endpoint, content);
			return await ProcessResponse<T>(response);
		}

		public async Task<ApiDataResponse<T>> GetAsync<T>(string endpoint)
		{
			var response = await _client.GetAsync(endpoint);
			return await ProcessResponse<T>(response);
		}
	}
}
