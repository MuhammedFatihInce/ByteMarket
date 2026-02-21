using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult Create()
		{
			return View();
		} 

		[HttpPost]
		public async Task<IActionResult> Create(CreateProductViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _productService.AddProductAsync(model);

			if (result)
			{
				TempData["SuccessMessage"] = "Ürün başarıyla listeye eklendi!";
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError("", "Ekleme işlemi sırasında bir sorun oluştu.");
			return View(model);
		}

		public async Task<IActionResult> AdminIndex()
		{
			var products = await _productService.GetProductsForAdminAsync();
			return View(products);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var response = await _apiService.GetByIdAsync<UpdateProductViewModel>("Products/GetById", id);
			if (!response.Success) return RedirectToAction("AdminIndex");

			return View(response.Data);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UpdateProductViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _productService.UpdateProductWithImagesAsync(model);
			if (result)
			{
				TempData["Success"] = "Ürün başarıyla güncellendi.";
				return RedirectToAction("AdminIndex");
			}

			ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu.");
			return View(model);
		}
	}
}
