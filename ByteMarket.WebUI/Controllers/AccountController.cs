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

	}
}
