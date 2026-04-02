using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
		private readonly IBasketService _basketService;

		public HeaderViewComponent(IBasketService basketService)
		{
			_basketService = basketService;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			int basketCount = 0;

			if (User.Identity.IsAuthenticated)
			{
				var result = await _basketService.GetBasketItems();

				if (result != null && result.Success && result.Data != null)
				{
					basketCount = result.Data.BasketItem.Sum(x => x.Quantity);

				}
			}

			

			return View(basketCount); 
		}
	}
}
