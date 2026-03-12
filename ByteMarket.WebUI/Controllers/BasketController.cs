using ByteMarket.WebUI.Models.Basket;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	[Authorize]
	public class BasketController : Controller
	{
		private readonly IBasketService _basketService;
		private readonly ICurrencyService _currencyService;
		public BasketController(IBasketService basketService, ICurrencyService currencyService)
		{
			_basketService = basketService;
			_currencyService = currencyService;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([FromBody] CreateBasketViewModel model)
		{
			var result = await _basketService.AddItemToBasket(model);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var result = await _basketService.GetBasketItems();

			if (result.Success)
			{
				return View(result.Data);
			}

			TempData["Error"] = result.Message;
			return View(new ListBasketViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateQuantity([FromBody] UpdateBasketItemQuantityViewModel model)
		{
			var result = await _basketService.UpdateQuantity(model);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}


		[HttpDelete("Basket/Delete/{basketItemId}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string basketItemId)
		{
			var result = await _basketService.RemoveBasketItem(basketItemId);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ApplyCouponToBasket([FromBody]string couponCode)
		{
			var result = await _basketService.ApplyCouponToBasketAsync(couponCode);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete("Basket/RemoveCouponFromBasket/{couponId}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveCouponFromBasket(string couponId)
		{
			var result = await _basketService.RemoveCouponFromBasket(couponId);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet("Basket/GetCurrency/{code}")]
		public async Task<IActionResult> GetCurrencyByCode([FromRoute]string code)
		{
			var result = await _currencyService.GetbyCode(code);

			return Json(new { data = result.Data , success = result.Success, message = result.Message });
		}
	}
}
