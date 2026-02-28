using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Models.Role;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class RoleService : IRoleService
	{
		private readonly IApiService _apiService;

		public RoleService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ApiDataResponse<List<RoleListViewModel>>> GetAllRolesAsync()
		{
			return await _apiService.GetAsync<List<RoleListViewModel>>("Roles/GetRoles");
		}

		public async Task<ApiDataResponse<object>> CreateRoleAsync(string roleName)
		{
			return await _apiService.PostAsync<object>($"Roles/CreateRole?name={roleName}", null);
		}

		public async Task<ApiDataResponse<object>> DeleteRoleAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("Roles/DeleteRole", id);
		}

		public async Task<ApiDataResponse<object>> AssignRoleAsync(AssignRoleViewModel model)
		{
			return await _apiService.PostAsync<object>("Roles/AssignRole", model);
		}
	}
}
