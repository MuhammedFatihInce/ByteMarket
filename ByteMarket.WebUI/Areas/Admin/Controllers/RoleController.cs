using ByteMarket.WebUI.Areas.Admin.Models.Role;
using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Areas.Admin.Controllers
{
	[Area("Admin"), Authorize(Policy = AuthorizePolicies.FullRoleManagement)]
	public class RoleController : Controller
	{
		private readonly IRoleService _roleService;
		private readonly IUserService _userService;

		public RoleController(IRoleService roleService, IUserService userService)
		{
			_roleService = roleService;
			_userService = userService;
		}

		public async Task<IActionResult> Index()
		{
			var usersResult = await _userService.GetAllUsersWithRolesAsync();

			var allRolesResult = await _roleService.GetAllRolesAsync();

			ViewBag.AllPermissions = AuthorizePolicies.GetSystemPermissions();

			return View((usersResult.Data, allRolesResult.Data));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(string roleName, List<string> permissions)
		{
			if (string.IsNullOrWhiteSpace(roleName)) return RedirectToAction("Index");

			var result = await _roleService.CreateRoleAsync(roleName);

			if (result.Success)
			{
				if (permissions != null && permissions.Any())
				{
					var roles = await _roleService.GetAllRolesAsync();
					var newRole = roles.Data?.FirstOrDefault(r => r.Name == roleName);

					if (newRole != null)
					{
						var updateModel = new PermissionsUpdateViewModel
						{
							Id = newRole.Id,
							Permissions = permissions
						};
						await _roleService.UpdatePermissions(updateModel);
					}
				}
				TempData["SuccessMessage"] = result.Message;
			}
			else
			{
				TempData["ErrorMessage"] = result.Message;
			}

			return RedirectToAction("Index");
		}

		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _roleService.DeleteRoleAsync(id);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdatePermissions([FromBody] PermissionsUpdateViewModel model)
		{
			var result = await _roleService.UpdatePermissions(model);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpGet]
		public async Task<IActionResult> GetPermissions(string roleId)
		{
			var result = await _roleService.GetPermissionsByRoleIdAsync(roleId);

			if (result.Success)
			{
				return Json(new { success = true, data = result.Data });
			}
			return Json(new { success = false, message = result.Message ?? "Yetkiler alınamadı." });
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsersForSelect([FromQuery] string q)
		{

			var users = await _userService.GetAllUsersByFilterAsync(q);

			if (users?.Data == null)
			{
				return Json(new List<object>());
			}

			var result = users.Data.Select(u =>
			{
				return new
				{
					id = u.Id,
					text = u.NameSurname,
					email = u.Email,
					roleIds = u.RoleIds
				};
			}).ToList();

			return Json(result);
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
