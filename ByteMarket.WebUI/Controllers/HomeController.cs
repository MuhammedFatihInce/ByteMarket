using ByteMarket.WebUI.Models;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ByteMarket.WebUI.Controllers
{
	public class HomeController : Controller
	{

		private readonly IProductService _productService;
		public HomeController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> Index(string? categoryId)
		{
			var result = await _productService.GetProductsForCustomerAsync(categoryId);

			if (result.Success)
			{
				ViewBag.ActiveCategoryId = categoryId;
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(result.Data);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
