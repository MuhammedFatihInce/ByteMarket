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
	}
}
