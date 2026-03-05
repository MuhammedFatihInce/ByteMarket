
namespace ByteMarket.Business.DTOs.Role
{
	public class RolePermissionsDto
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public List<PermissionCheckDto> Permissions { get; set; } = new();
	}
}
