using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ByteMarket.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly IApiService _apiService;

		public ProductController(IProductService productService, IApiService apiService)
		{
			_productService = productService;
			_apiService = apiService;
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var categories = await _apiService.GetAllAsync<ListCategoryViewModel>("Category/GetAll");

			var model = new CreateProductViewModel
			{
				CategoryList = categories.Data.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList()
			};

			return View(model);
		} 

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var categories = await _apiService.GetAllAsync<ListCategoryViewModel>("Category/GetAll");
				model.CategoryList = categories.Data.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();

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

			var allCategories = await _apiService.GetAllAsync<ListCategoryViewModel>("Category/GetAll");

			var model = new UpdateProductViewModel
			{
				Id = product.Id.ToString(),
				Name = product.Name,
				Stock = product.Stock,
				Price = product.Price,
				
				CategoryIds = product.Categories.Select(c => c.Id.ToString()).ToList(),

				CategoryList = allCategories.Data.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList(),

				ProductImageFiles = product.ProductImageFiles
				
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateProductViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _productService.UpdateProductWithImagesAsync(model);
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
