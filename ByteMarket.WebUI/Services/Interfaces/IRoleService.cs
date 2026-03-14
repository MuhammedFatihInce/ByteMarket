using ByteMarket.WebUI.Areas.Admin.Models.Role;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IRoleService
	{
		Task<ApiDataResponse<List<RoleListViewModel>>> GetAllRolesAsync();
		Task<ApiDataResponse<object>> CreateRoleAsync(string roleName);
		Task<ApiDataResponse<object>> DeleteRoleAsync(string roleName);
		Task<ApiDataResponse<object>> AssignRoleAsync(BulkAssignRoleViewModel model);
		Task<ApiDataResponse<object>> UpdatePermissions(PermissionsUpdateViewModel model);
		Task<ApiDataResponse<RolePermissionsViewModel>> GetPermissionsByRoleIdAsync(string roleId);
	}
}
