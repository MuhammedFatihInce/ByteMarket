using ByteMarket.Business.DTOs.User;
using ByteMarket.WebUI.Models.Auth;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebUI.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Login() 
		{ 
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			var result = await _accountService.LoginAsync(model);
			if (result.Success)
				return RedirectToAction("Index", "Home");

			ModelState.AddModelError(string.Empty, result.Message);
			return View(model);
		}

		public IActionResult Logout()
		{
			_accountService.Logout();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		} 

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var result = await _accountService.RegisterAsync(model);
			if (result.Success)
			{
				TempData["RegisterSuccess"] = "Kaydınız başarıyla tamamlandı. Giriş yapabilirsiniz.";
				return RedirectToAction("Login");
			}

			ModelState.AddModelError(string.Empty, result.Message);
			return View(model);
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
		{
			if (string.IsNullOrEmpty(idToken)) return Json(new { success = false, message = "Token boş olamaz." });

			var result = await _accountService.GoogleLoginAsync(idToken);

			if (result.Success)
			{
				return Json(new { success = true });
			}

			return Json(new { success = false, message = result.Message });
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> ForgotPassword([FromBody] string email)
		{
			var result = await _accountService.PasswordResetAsync(email);
			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token)) return RedirectToAction("Login");

			var model = new ResetPasswordViewModel { Email = email, Token = token };
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
		{
			var result = await _accountService.VerifyResetTokenAsync(model);
			return Json(new { success = result.Success, message = result.Message });
		}

	}
}
