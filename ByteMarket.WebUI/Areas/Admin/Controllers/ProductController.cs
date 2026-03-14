using ByteMarket.WebUI.Areas.Admin.Models.Product;
using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Areas.Admin.Controllers
{
	
	[Area("Admin"), Authorize(Policy = AuthorizePolicies.FullProductManagement)]
	public class ProductController : Controller
	{

		private readonly IProductService _productService;
		private readonly IApiService _apiService;
		private readonly ICategoryService _categoryService;
		private readonly IEditorService _editorService;

		public ProductController(IProductService productService, IApiService apiService, ICategoryService categoryService, IEditorService editorService)
		{
			_productService = productService;
			_apiService = apiService;
			_categoryService = categoryService;
			_editorService = editorService;
		}

		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetProductsForAdminAsync();
			return View(products.Data);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new CreateProductViewModel
			{
				CategoryList = await _categoryService.GetCategorySelectListAsync()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.CategoryList = await _categoryService.GetCategorySelectListAsync();
				return View(model);
			}

			var result = await _productService.AddProductAsync(model);

			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("Index");
			}

			ModelState.AddModelError(String.Empty, result.Message);

			model.CategoryList = await _categoryService.GetCategorySelectListAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var productResult = await _apiService.GetByIdAsync<SingleProductAdminViewModel>("Product", id);
			if (!productResult.Success) return RedirectToAction("Index");

			var product = productResult.Data;

			var model = new UpdateProductViewModel
			{
				Id = product.Id.ToString(),
				Name = product.Name,
				Stock = product.Stock,
				Price = product.Price,
				Description = product.Description,
				CategoryIds = product.Categories.Select(c => c.Id.ToString()).ToList(),
				CategoryList = await _categoryService.GetCategorySelectListAsync(),
				ProductImageFiles = product.ProductImageFiles
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateProductViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.CategoryList = await _categoryService.GetCategorySelectListAsync();
				return View(model);
			}

			if (model.OrderedImageIds.Count == 1 && model.OrderedImageIds[0].Contains(","))
			{
				model.OrderedImageIds = model.OrderedImageIds[0]
					.Split(',', StringSplitOptions.RemoveEmptyEntries)
					.ToList();
			}

			var result = await _productService.UpdateProductWithImagesAsync(model);
			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("Index");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _productService.DeleteProductAsync(id);

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
			var result = await _productService.DeleteProductImageAsync(id);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UploadEditorImage(IFormFile file)
		{
			var result = await _editorService.Upload(file);
			return Json(new { url = result.Data, success = result.Success, message = result.Message });
		}

		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteEditorImage(string url)
		{
			var result = await _editorService.Delete(url);
			return Json(new { success = result.Success, message = result.Message });
		}

	}
}
