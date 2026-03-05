using ByteMarket.WebUI.Models.Auth;
using ByteMarket.WebUI.Models.ResultModels;
using System.Text.Json;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IAccountService
	{
		Task<ApiDataResponse<JsonElement>> LoginAsync(LoginViewModel loginViewModel);
		Task Logout();
		Task<ApiDataResponse<object>> RegisterAsync(RegisterViewModel registerViewModel);
		Task<(bool IsSuccess, string? NewToken)> RefreshTokenAsync();
		Task<ApiDataResponse<JsonElement>> GoogleLoginAsync(string idToken);
		Task<ApiDataResponse<object>> PasswordResetAsync(string email);
		Task<ApiDataResponse<object>> VerifyResetTokenAsync(ResetPasswordViewModel model);
	}
}
