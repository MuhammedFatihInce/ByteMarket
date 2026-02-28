using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Models.Role;
using ByteMarket.WebUI.Models.User;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.AdminOnly)]
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

			ViewBag.AllRoles = allRolesResult.Data ?? new List<Models.Role.RoleListViewModel>();

			return View(usersResult.Data ?? new List<UserListViewModel>());
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
