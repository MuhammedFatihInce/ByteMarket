using ByteMarket.WebUI.Models.Category;
using ByteMarket.WebUI.Models.ResultModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<ApiDataResponse<string>> AddCategoryAsync(CreateCategoryViewModel model);
		Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesForAdminAsync();
		Task<ApiDataResponse<object>> UpdateCategoriesAsync(UpdateCategoryViewModel model);
		Task<List<SelectListItem>> GetCategorySelectListAsync();
		Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesAsync();
		Task<ApiDataResponse<object>> DeleteCategoryAsync(string id);
		Task<ApiDataResponse<object>> DeleteCategoryImageAsync(string id);
	}
}
