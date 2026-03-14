using ByteMarket.WebUI.Areas.Admin.Models.User;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class UserService : IUserService
	{
		private readonly IApiService _apiService;

		public UserService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<List<UserListViewModel>>> GetAllUsersWithRolesAsync()
		{
			return await _apiService.GetAllAsync<UserListViewModel>("User");
		}
		public async Task<ApiDataResponse<List<GetAllUsersByFilterViewModel>>> GetAllUsersByFilterAsync(string q)
		{

			return await _apiService.GetAllAsync<GetAllUsersByFilterViewModel>($"User/GetAllUsersByFilter?q={q}");
		}
		public async Task<ApiDataResponse<List<UserListViewModel>>> GetAllUsersWithRolesAsync(string roleName)
		{
			return await _apiService.GetAllAsync<UserListViewModel>($"User/GetAllUsersByRole/{roleName}");
		}
	}
}
