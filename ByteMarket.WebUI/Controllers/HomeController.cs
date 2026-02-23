using ByteMarket.WebUI.Models;
using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ByteMarket.WebUI.Controllers
{
	public class HomeController : Controller
	{

		private readonly IApiService _apiService;
		public HomeController(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _apiService.GetAllAsync<ProductListViewModel>("Product/GetAll");

			if (result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(new List<ProductListViewModel>());
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
