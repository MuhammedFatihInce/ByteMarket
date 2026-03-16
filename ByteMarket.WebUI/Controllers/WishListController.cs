using ByteMarket.WebUI.Models.wishList;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	[Authorize]
	public class WishListController : Controller
	{
		private readonly IWishListService _wishListService;
		public WishListController(IWishListService wishListService)
		{
			_wishListService = wishListService;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([FromBody] string productId)
		{
			var result = await _wishListService.AddWishListProductAsync(productId);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var result = await _wishListService.GetAllWishListProductsAsync();

			if (result != null && result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result != null ? result.Message : "İstek listesi boş." ;
			return View(new List<WishListProductViewModel>());
		}

		[HttpDelete("WishList/Delete/{productId}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string productId)
		{
			var result = await _wishListService.DeleteWishListAsync(productId);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}
	}
}
