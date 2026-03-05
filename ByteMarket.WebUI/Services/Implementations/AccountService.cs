using ByteMarket.WebUI.Models.Auth;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;
using ByteMarket.WebUI.Utilities.Helpers.Auth;
using System.Net;
using System.Text.Json;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IApiService _apiService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
		private readonly IAuthHelper _authHelper;

		public AccountService(IApiService apiService, IHttpContextAccessor httpContextAccessor, IAuthHelper authHelper)
		{
			_apiService = apiService;
			_httpContextAccessor = httpContextAccessor;
			_authHelper = authHelper;
		}

		public async Task<ApiDataResponse<JsonElement>> LoginAsync(LoginViewModel loginViewModel)
		{
			var result = await _apiService.PostAsync<JsonElement>("auth/login", loginViewModel);

			if (result.Success && result.Data.ValueKind != JsonValueKind.Null)
			{
				await HandleTokenResponse(result.Data, loginViewModel.RememberMe);
			}
			return result;
		}

		public async Task Logout()
		{
			var context = _httpContextAccessor.HttpContext;
			if (context == null) return;

			var refreshToken = context.Request.Cookies["refreshToken"];

			await _authHelper.SignOutUserAsync();

			context.Response.Cookies.Delete("jwt");
			context.Response.Cookies.Delete("refreshToken");

			if (!string.IsNullOrEmpty(refreshToken))
			{
				var encodedToken = WebUtility.UrlEncode(refreshToken);
				await _apiService.PostAsync<object>($"auth/logout?refreshToken={encodedToken}", null);
			}

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

		public async Task<(bool IsSuccess, string? NewToken)> RefreshTokenAsync()
		{
			await _semaphore.WaitAsync();
			try
			{
				var context = _httpContextAccessor.HttpContext;
				if (context == null) return (false, null);

				var refreshToken = context.Request.Cookies["refreshToken"];
				if (string.IsNullOrEmpty(refreshToken)) return (false, null);

				var encodedToken = WebUtility.UrlEncode(refreshToken);
				var result = await _apiService.PostAsync<JsonElement>($"auth/refresh-token-login?refreshToken={encodedToken}", null);

				if (result.Success && result.Data.ValueKind != JsonValueKind.Null)
				{
					context.Response.Cookies.Delete("refreshToken");
					var newAccessToken = await HandleTokenResponse(result.Data);
					return (true, newAccessToken);
				}
				return (false, null);
			}
			finally
			{
				_semaphore.Release();
			}
		}

		private async Task<string?> HandleTokenResponse(JsonElement data, bool isPersistent = false)
		{
			var context = _httpContextAccessor.HttpContext;
			if (context == null) return null;

			
			var accessToken = data.GetProperty("accessToken").GetString();
			var expiration = data.GetProperty("expiration").GetDateTime().ToUniversalTime();
			var refreshToken = data.GetProperty("refreshToken").GetString();
			var refreshTokenExp = data.GetProperty("refreshTokenExpiration").GetDateTime().ToUniversalTime();

			if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) return null;

			context.Response.Cookies.Append("jwt", accessToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = expiration
			});

			context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = refreshTokenExp
			});

			await _authHelper.SignInUserAsync(accessToken, refreshTokenExp, isPersistent);

			return accessToken;
		}

		public async Task<ApiDataResponse<JsonElement>> GoogleLoginAsync(string idToken)
		{
			var result = await _apiService.PostAsync<JsonElement>("Auth/google-login", idToken);

			if (result.Success && result.Data.ValueKind != JsonValueKind.Null)
			{
				await HandleTokenResponse(result.Data, true);
			}

			return result;
		}

		public async Task<ApiDataResponse<object>> PasswordResetAsync(string email)
		{
			return await _apiService.PostAsync<object>("Auth/password-reset", email);
		}

		public async Task<ApiDataResponse<object>> VerifyResetTokenAsync(ResetPasswordViewModel model)
		{
			
			var resetData = new
			{
				Email = model.Email ?? "",
				Token = model.Token ?? "",
				NewPassword = model.Password 
			};

			return await _apiService.PostAsync<object>("Auth/verify-reset-token", resetData);
		}
	}
}
