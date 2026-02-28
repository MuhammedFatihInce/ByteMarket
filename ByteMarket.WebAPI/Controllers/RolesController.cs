using ByteMarket.Business.Abstract;
using ByteMarket.Business.Constants;
using ByteMarket.Business.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.AdminOnly)]
	public class RolesController : BaseController
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet("GetRoles")]
		public async Task<IActionResult> GetRoles()
		{
			var result = await _roleService.GetAllRolesAsync();
			return CreateActionResult(result);
		}

		[HttpPost("CreateRole")]
		public async Task<IActionResult> CreateRole([FromQuery] string name)
		{
			var result = await _roleService.CreateRoleAsync(name);
			return CreateActionResult(result);
		}

		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRole(AssignRoleDto assignRoleDto)
		{
			var result = await _roleService.AssignRoleToUserAsync(assignRoleDto);
			return CreateActionResult(result);
		}

		[HttpDelete("DeleteRole/{id}")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			var result = await _roleService.DeleteRoleAsync(id);
			return CreateActionResult(result);
		}
	}
}
