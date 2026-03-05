
using ByteMarket.Business.DTOs.Role;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IRoleService
	{
		Task<IResult> CreateRoleAsync(string name);
		Task<IResult> DeleteRoleAsync(string id);
		Task<IDataResult<List<RoleListDto>>> GetAllRolesAsync();
		Task<IResult> AssignRoleToUserAsync(AssignRoleDto assignRoleDto);
		Task<IResult> UpdatePermissions(PermissionsUpdateDto dto);
		Task<IDataResult<RolePermissionsDto>> GetPermissionsByRoleIdAsync(string roleId);
	}
}
