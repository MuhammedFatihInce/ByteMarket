
using ByteMarket.WebUI.Areas.Admin.Models.Category;
using ByteMarket.WebUI.Models.Category;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class CategoryService:ICategoryService
	{
		private readonly IApiService _apiService;
		public CategoryService(IApiService apiService) => _apiService = apiService;

		public async Task<ApiDataResponse<string>> AddCategoryAsync(CreateCategoryViewModel model)
		{
			var categoryResponse = await _apiService.PostAsync<string>("Category", new
			{
				model.Name, 
				model.Icon
			});

			if (!categoryResponse.Success) return categoryResponse;

			string newCategoryId = categoryResponse.Data;

			if (model.File != null)
			{
				var imageResponse = await UploadImageInternalAsync(newCategoryId, model.File);

				if (!imageResponse.Success)
				{
					imageResponse.Message = $"Kategori eklendi fakat resimler yüklenemedi: {imageResponse.Message}";
					return new ApiDataResponse<string> { Success = false, Message = imageResponse.Message, Data = newCategoryId };
				}
			}

			return categoryResponse;
		}

		public async Task<ApiDataResponse<List<ListCategoryAdminViewModel>>> GetCategoriesForAdminAsync()
		{
			return await _apiService.GetAllAsync<ListCategoryAdminViewModel>("Category");
		}

		public async Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesAsync()
		{
			return await _apiService.GetAllAsync<ListCategoryViewModel>("Category");
		}

		public async Task<ApiDataResponse<object>> UpdateCategoriesAsync(UpdateCategoryViewModel model)
		{
			var updateResponse = await _apiService.PutAsync<object>("Category", new
			{
				model.Id,
				model.Name,
				model.Icon
			});

			if (!updateResponse.Success) return updateResponse;

			if (model.File != null)
			{
				var imageResponse = await UploadImageInternalAsync(model.Id, model.File);
				if (!imageResponse.Success) return imageResponse;
			}

			return updateResponse;
		}

		public async Task<List<SelectListItem>> GetCategorySelectListAsync()
		{
			var result = await _apiService.GetAllAsync<ListCategoryViewModel>("Category");

			if (result?.Data == null) return new List<SelectListItem>();

			return result.Data.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name
			}).ToList();
		}

		private async Task<ApiDataResponse<object>> UploadImageInternalAsync(string categoryId, IFormFile file)
		{
			var content = new MultipartFormDataContent();

			var streamContent = new StreamContent(file.OpenReadStream());
			streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
			content.Add(streamContent, "file", file.FileName);

			return await _apiService.PostMultipartAsync<object>($"CategoryImage/Upload/{categoryId}", content);
		}

		public async Task<ApiDataResponse<object>> DeleteCategoryAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("Category", id);
		}

		public async Task<ApiDataResponse<object>> DeleteCategoryImageAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("CategoryImage", id);
		}
	}
}
