using ByteMarket.WebUI.Models.Auth;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;
using System.Text.Json;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IApiService _apiService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AccountService(IApiService apiService, IHttpContextAccessor httpContextAccessor)
		{
			_apiService = apiService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ApiDataResponse<JsonElement>> LoginAsync(LoginViewModel loginViewModel)
		{
			var result = await _apiService.PostAsync<JsonElement>("auth/login", loginViewModel);

			if (result.Success && result.Data.ValueKind != JsonValueKind.Null)
			{
				if (result.Data.TryGetProperty("accessToken", out var tokenElement))
				{
					var token = tokenElement.GetString();

					var cookieOptions = new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict,
						Expires = DateTime.UtcNow.AddMinutes(60)
					};

					_httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, cookieOptions);
				}

				if (result.Data.TryGetProperty("refreshToken", out var refreshElement))
				{
					var refreshToken = refreshElement.GetString();
					_httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict,
						Expires = DateTime.UtcNow.AddDays(7)
					});
				}
			}

			return result;
		}

		public async Task Logout()
		{
			var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

			if (!string.IsNullOrEmpty(refreshToken))
			{
				await _apiService.PostAsync<object>($"auth/logout?refreshToken={refreshToken}", null);
			}
			_httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt");
			_httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
		}

		public async Task<ApiDataResponse<object>> RegisterAsync(RegisterViewModel registerViewModel)
		{
			return await _apiService.PostAsync<object>("auth/register", new
			{
				registerViewModel.NameSurname,
				registerViewModel.Username,
				registerViewModel.Email,
				registerViewModel.Password
			});
		}
	}
}
