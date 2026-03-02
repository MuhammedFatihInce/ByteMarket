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
		public async Task<IActionResult> Detail(string id)
		{
			var result = await _productService.GetProductDetailsAsync(id);

			if (!result.Success) return RedirectToAction("Index", "Home");

			return View(result.Data);
		}
	}
}
