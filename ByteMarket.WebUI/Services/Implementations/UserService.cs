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
	}
}
