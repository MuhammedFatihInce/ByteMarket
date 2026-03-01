using ByteMarket.Business.Abstract;
using ByteMarket.Business.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.AdminOnly)]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("GetAllUsers")]
		public async Task<IActionResult> GetAllUsers()
		{
			var result = await _userService.GetAllUsersWithRolesAsync();
			return CreateActionResult(result);
		}
	}
}
