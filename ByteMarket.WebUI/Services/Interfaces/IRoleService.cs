using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Models.Role;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IRoleService
	{
		Task<ApiDataResponse<List<RoleListViewModel>>> GetAllRolesAsync();
		Task<ApiDataResponse<object>> CreateRoleAsync(string roleName);
		Task<ApiDataResponse<object>> DeleteRoleAsync(string roleName);
	}
}
