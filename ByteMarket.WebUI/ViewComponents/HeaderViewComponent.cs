using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
		private readonly ICategoryService _categoryService;
		public HeaderViewComponent(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = new List<ListCategoryViewModel>();

			var result = await _categoryService.GetCategoriesAsync();

			if (result.Success && result.Data != null)
			{
				return View(result.Data);
			}

			return View(categories); 
		}
	}
}
