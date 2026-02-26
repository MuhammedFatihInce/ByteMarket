using ByteMarket.WebUI.Models.Product;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly IApiService _apiService;
		private readonly ICategoryService _categoryService;

		public ProductController(IProductService productService, IApiService apiService, ICategoryService categoryService)
		{
			_productService = productService;
			_apiService = apiService;
			_categoryService = categoryService;
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
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		public async Task<IActionResult> AdminIndex()
		{
			var products = await _productService.GetProductsForAdminAsync();
			return View(products.Data);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var productResult = await _apiService.GetByIdAsync<SingleProductViewModel>("Product/GetById", id);
			if (!productResult.Success) return RedirectToAction("AdminIndex");

			var product = productResult.Data;

			var model = new UpdateProductViewModel
			{
				Id = product.Id.ToString(),
				Name = product.Name,
				Stock = product.Stock,
				Price = product.Price,
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
			
			var result = await _productService.UpdateProductWithImagesAsync(model);
			if (result.Success)
			{
				TempData["SuccessMessage"] = result.Message;
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError(String.Empty, result.Message);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Detail(string id)
		{
			var result = await _productService.GetProductDetailsAsync(id);

			if (!result.Success) return RedirectToAction("Index", "Home");

			return View(result.Data);
		}
	}
}
