using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IApiService _apiService;
		public CategoryController(ICategoryService categoryService, IApiService apiService)
		{
			_categoryService = categoryService;	
			_apiService = apiService;
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCategoryViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _categoryService.AddCategoryAsync(model);

			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		public async Task<IActionResult> AdminIndex()
		{
			var categories = await _categoryService.GetCategoriesForAdminAsync();
			return View(categories.Data);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var response = await _apiService.GetByIdAsync<UpdateCategoryViewModel>("Category/GetById", id);
			if (!response.Success) return RedirectToAction("AdminIndex");

			return View(response.Data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _categoryService.UpdateCategoriesAsync(model);
			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

	}
}
