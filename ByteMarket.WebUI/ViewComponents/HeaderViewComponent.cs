using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
