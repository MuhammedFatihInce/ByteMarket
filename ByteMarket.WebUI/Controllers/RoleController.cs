using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Models.Role;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.AdminOnly)]
	public class RoleController : Controller
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _roleService.GetAllRolesAsync();
			return View(result.Data ?? new List<RoleListViewModel>());
		}

		[HttpPost]
		public async Task<IActionResult> Create(string roleName)
		{
			if (string.IsNullOrWhiteSpace(roleName)) return RedirectToAction("Index");

			var result = await _roleService.CreateRoleAsync(roleName);
			if (result.Success) TempData["SuccessMessage"] = result.Message;
			else TempData["ErrorMessage"] = result.Message;

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _roleService.DeleteRoleAsync(id);

			if (result.Success)
			{
				return Json(new { success = true, message = result.Message });
			}

			return Json(new { success = false, message = result.Message });
		}
	}
}
