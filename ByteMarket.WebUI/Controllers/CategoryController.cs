using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Models.Category;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Policy = AuthorizePolicies.AdminOnly)]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Policy = AuthorizePolicies.AdminOnly)]
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

		[Authorize(Policy = AuthorizePolicies.AdminOnly)]
		public async Task<IActionResult> AdminIndex()
		{
			var categories = await _categoryService.GetCategoriesForAdminAsync();
			return View(categories.Data);
		}

		[HttpGet]
		[Authorize(Policy = AuthorizePolicies.AdminOnly)]
		public async Task<IActionResult> Edit(string id)
		{
			var categoryResult = await _apiService.GetByIdAsync<SingleCategoryViewModel>("Category/GetById", id);

			if (!categoryResult.Success) return RedirectToAction("AdminIndex");

			var category = categoryResult.Data;

			var model = new UpdateCategoryViewModel
			{
				Id = category.Id.ToString(),
				Name = category.Name,
				Icon = category.Icon,
				CategoryImageFile = category.CategoryImageFile
			};
			
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = AuthorizePolicies.AdminOnly)]
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
