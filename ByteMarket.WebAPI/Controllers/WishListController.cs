using ByteMarket.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize]
	public class WishListController : BaseController
	{
		private readonly IWishListService _wishListService;
		public WishListController(IWishListService wishListService)
		{
			_wishListService = wishListService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _wishListService.GetAllWishListProductsAsync(userId);
			return CreateActionResult(result);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] string productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _wishListService.AddWishlistProductAsync(userId, productId);
			return CreateActionResult(result, successStatusCode:201);
		}

		[HttpDelete("{productId}")]
		public async Task<IActionResult> Delete(string productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _wishListService.RemoveWishListProductAsync(userId, productId);
			return CreateActionResult(result);
		}
	}
}
