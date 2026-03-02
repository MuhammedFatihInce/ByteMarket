using ByteMarket.WebUI.Areas.Admin.Models.Role;
using ByteMarket.WebUI.Areas.Admin.Models.User;
using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Areas.Admin.Controllers
{
	[Area("Admin"), Authorize(Policy = AuthorizePolicies.AdminOnly)]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly IRoleService _roleService;

		public UserController(IUserService userService, IRoleService roleService)
		{
			_userService = userService;
			_roleService = roleService;
		}

		public async Task<IActionResult> Index()
		{
			var usersResult = await _userService.GetAllUsersWithRolesAsync();

			var allRolesResult = await _roleService.GetAllRolesAsync();

			return View((usersResult.Data, allRolesResult.Data));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssignRole([FromBody] AssignRoleViewModel model)
		{
			if (model == null || string.IsNullOrEmpty(model.UserId))
				return Json(new { success = false, message = "Geçersiz veri gönderildi." });

			var result = await _roleService.AssignRoleAsync(model);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}
	}
}
