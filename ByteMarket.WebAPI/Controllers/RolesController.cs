using ByteMarket.Business.Abstract;
using ByteMarket.Entities.Constants;
using ByteMarket.Business.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.FullRoleManagement)]
	public class RolesController : BaseController
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet]
		public async Task<IActionResult> GetRoles()
		{
			var result = await _roleService.GetAllRolesAsync();
			return CreateActionResult(result);
		}

		[HttpPost]
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			var result = await _roleService.DeleteRoleAsync(id);
			return CreateActionResult(result);
		}

		[HttpPost("UpdatePermissions")]
		public async Task<IActionResult> UpdatePermissions(PermissionsUpdateDto dto)
		{
			var result = await _roleService.UpdatePermissions(dto);
			return CreateActionResult(result);
		}

		[HttpGet("GetPermissions/{roleId}")]
		public async Task<IActionResult> GetPermissionsByRoleIdAsync(string roleId)
		{
			var result = await _roleService.GetPermissionsByRoleIdAsync(roleId);
			return CreateActionResult(result);
		}
	}
}
