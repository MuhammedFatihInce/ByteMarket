
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ByteMarket.WebAPI.Controllers
{
	public class AuthController : BaseController
	{
		private readonly IUserService _userService;
		private readonly IAuthService _authService;

		public AuthController(IUserService userService, IAuthService authService)
		{
			_userService = userService;
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(CreateUserDto createUserDto)
		{
			var result = await _userService.CreateAsync(createUserDto);
			return CreateActionResult(result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserDto loginUserDto)
		{
			var result = await _authService.LoginAsync(loginUserDto);

			return CreateActionResult(result, errorStatusCode: 401);
		}

		[AllowAnonymous]
		[HttpPost("refresh-token-login")]
		public async Task<IActionResult> RefreshTokenLogin([FromQuery] string refreshToken)
		{
			var result = await _authService.RefreshTokenLoginAsync(refreshToken);

			return CreateActionResult(result, errorStatusCode: 401);
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromQuery] string refreshToken)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _authService.LogoutAsync(userId, refreshToken);
			return CreateActionResult(result);
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
		{
			var result = await _authService.GoogleLoginAsync(idToken);
			return CreateActionResult(result);
		}

		[HttpPost("password-reset")]
		public async Task<IActionResult> PasswordReset([FromBody] string email)
		{
			var result = await _authService.PasswordResetAsync(email);
			return CreateActionResult(result);
		}

		[HttpPost("verify-reset-token")]
		public async Task<IActionResult> VerifyResetToken([FromBody] ResetPasswordDto resetPasswordDto)
		{
			var result = await _authService.VerifyResetTokenAsync(resetPasswordDto);
			return CreateActionResult(result);
		}
	}
}
