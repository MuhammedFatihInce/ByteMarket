using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<ApiDataResponse<object>> AddCategoryAsync(CreateCategoryViewModel model);
		Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesForAdminAsync();
		Task<ApiDataResponse<object>> UpdateCategoriesAsync(UpdateCategoryViewModel model);
	}
}
