using ByteMarket.WebUI.Areas.Admin.Models.Role;
using ByteMarket.WebUI.Models.ResultModels;
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
			return await _apiService.GetAsync<List<RoleListViewModel>>("Roles");
		}

		public async Task<ApiDataResponse<object>> CreateRoleAsync(string roleName)
		{
			return await _apiService.PostAsync<object>($"Roles?name={roleName}", null);
		}

		public async Task<ApiDataResponse<object>> DeleteRoleAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("Roles", id);
		}

		public async Task<ApiDataResponse<object>> AssignRoleAsync(AssignRoleViewModel model)
		{
			return await _apiService.PostAsync<object>("Roles/AssignRole", model);
		}

		public async Task<ApiDataResponse<object>> UpdatePermissions(PermissionsUpdateViewModel model)
		{
			return await _apiService.PostAsync<object>("Roles/UpdatePermissions", model);
		}

		public async Task<ApiDataResponse<RolePermissionsViewModel>> GetPermissionsByRoleIdAsync(string roleId)
		{
			return await _apiService.GetByIdAsync<RolePermissionsViewModel>("Roles/GetPermissions", roleId);
		}
	}
}
