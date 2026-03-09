using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ByteMarket.WebAPI.Controllers
{
	[Authorize]
	public class BasketsController : BaseController
	{
		readonly IBasketService _basketService;

		public BasketsController(IBasketService basketService)
		{
			_basketService = basketService;
		}

		[HttpPost]
		public async Task<IActionResult> AddItemToBasket(CreateBasketDto creaBasketDto)
		{
			var result = await _basketService.AddItemToBasketAsync(creaBasketDto);
			return CreateActionResult(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetBasketItems()
		{
			var result = await _basketService.GetBasketItemsAsync();
			return CreateActionResult(result);
		}

		[HttpDelete("{basketItemId}")]
		public async Task<IActionResult> RemoveBasketItem(string basketItemId)
		{
			var result = await _basketService.RemoveBasketItemAsync(basketItemId);
			return CreateActionResult(result);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateQuantity(UpdateBasketItemQuantityDto dto)
		{
			var result = await _basketService.UpdateQuantityAsync(dto);
			return CreateActionResult(result);
		}

		[HttpPost("ApplyCouponToBasket")]
		public async Task<IActionResult> ApplyCouponToBasket([FromBody]string couponCode)
		{
			var result = await _basketService.ApplyCouponToBasketAsync(couponCode);
			return CreateActionResult(result);
		}

		[HttpDelete("RemoveCouponFromBasket/{couponId}")]
		public async Task<IActionResult> RemoveCouponFromBasket(string couponId)
		{
			var result = await _basketService.RemoveCouponFromBasketAsync(couponId);
			return CreateActionResult(result);
		}

	}
}
