using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class CategoryService:ICategoryService
	{
		private readonly IApiService _apiService;
		public CategoryService(IApiService apiService) => _apiService = apiService;

		public async Task<ApiDataResponse<object>> AddCategoryAsync(CreateCategoryViewModel model)
		{
			var categoryResponse = await _apiService.PostAsync<object>("Category/Add", model);
			return categoryResponse;
		}

		public async Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesForAdminAsync()
		{
			return await _apiService.GetAllAsync<ListCategoryViewModel>("Category/GetAll");
		}

		public async Task<ApiDataResponse<object>> UpdateCategoriesAsync(UpdateCategoryViewModel model)
		{
			var updateResponse = await _apiService.PutAsync<object>("Category/Update", new
			{
				model.Id,
				model.Name,
				model.Icon
			});

			return updateResponse;
		}
	}
}
