using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
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

			var success = await _productService.AddProductAsync(model);
			if (success)
			{
				TempData["SuccessMessage"] = "Ürün başarıyla listeye eklendi!";
				return RedirectToAction("AdminIndex", "Product");
			}

			ViewBag.ErrorMessage = "Ürün eklenirken bir hata oluştu.";
			return View(model);
		}

		public async Task<IActionResult> AdminIndex()
		{
			var products = await _productService.GetProductsForAdminAsync();
			return View(products);
		}

	}
}
