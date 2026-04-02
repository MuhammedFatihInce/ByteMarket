using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	public class SearchController : Controller
	{
		private readonly IProductService _productService;

		public SearchController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProductsForSelect([FromQuery] string q)
		{
			var products = await _productService.GetAllProductByFilterAsync(q);

			if (products?.Data == null)
			{
				return Json(new List<object>());
			}

			var result = products.Data.Select(p => new
			{
				id = p.Id,
				text = p.Name
			}).ToList();

			return Json(result);
		}

	}
}
