
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Areas.Admin.ViewComponents
{
	[Area("Admin")]
	public class AdminHeaderViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
