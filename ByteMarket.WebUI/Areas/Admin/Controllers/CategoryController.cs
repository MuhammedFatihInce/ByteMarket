using ByteMarket.WebUI.Areas.Admin.Models.Category;
using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Areas.Admin.Controllers
{
	[Area("Admin"), Authorize(Policy = AuthorizePolicies.AdminOnly)]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IApiService _apiService;
		public CategoryController(ICategoryService categoryService, IApiService apiService)
		{
			_categoryService = categoryService;
			_apiService = apiService;
		}
		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetCategoriesForAdminAsync();
			return View(categories.Data);
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
				return RedirectToAction("Index");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var categoryResult = await _apiService.GetByIdAsync<SingleCategoryAdminViewModel>("Category", id);

			if (!categoryResult.Success) return RedirectToAction("Index");

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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _categoryService.UpdateCategoriesAsync(model);
			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("Index");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		[HttpDelete]
		[ValidateAntiForgeryToken] // AJAX içindeki header ile eşleşecek
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _categoryService.DeleteCategoryAsync(id);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteImage(string id)
		{
			var result = await _categoryService.DeleteCategoryImageAsync(id);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}
	}
}
