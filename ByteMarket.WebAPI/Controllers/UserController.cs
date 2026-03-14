using ByteMarket.Business.Abstract;
using ByteMarket.Entities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.FullUserManagement)]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var result = await _userService.GetAllUsersWithRolesAsync();
			return CreateActionResult(result);
		}

		[HttpGet("GetAllUsersByFilter")]
		public async Task<IActionResult> GetAllUsersByFilter([FromQuery] string q)
		{
			var result = await _userService.GetAllUsersByFilterAsync(q);
			return CreateActionResult(result);
		}


		[HttpGet("GetAllUsersByRole/{roleName}")]
		public async Task<IActionResult> GetAllUsersByRole(string roleName)
		{
			var result = await _userService.GetAllUsersByRoleAsync(roleName);
			return CreateActionResult(result);
		}
	}
}
