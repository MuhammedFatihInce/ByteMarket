
using ByteMarket.WebUI.Models.Category;
using ByteMarket.WebUI.Models.User;
using ByteMarket.WebUI.Services.Interfaces;
using ByteMarket.WebUI.Utilities.Helpers.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ByteMarket.WebUI.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
		private readonly ICategoryService _categoryService;
		private readonly IAuthHelper _authHelper; 

		public HeaderViewComponent(ICategoryService categoryService, IAuthHelper authHelper)
		{
			_categoryService = categoryService;
			_authHelper = authHelper;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = new List<ListCategoryViewModel>();

			var result = await _categoryService.GetCategoriesAsync();

			ViewBag.UserInfo = _authHelper.GetUserFromCookie();

			if (result.Success && result.Data != null)
			{
				return View(result.Data);
			}

			return View(categories); 
		}
	}
}
