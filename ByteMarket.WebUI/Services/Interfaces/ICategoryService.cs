using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Models.ResultModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<ApiDataResponse<object>> AddCategoryAsync(CreateCategoryViewModel model);
		Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesForAdminAsync();
		Task<ApiDataResponse<object>> UpdateCategoriesAsync(UpdateCategoryViewModel model);
		Task<List<SelectListItem>> GetCategorySelectListAsync();
		Task<ApiDataResponse<List<ListCategoryViewModel>>> GetCategoriesAsync();
	}
}
