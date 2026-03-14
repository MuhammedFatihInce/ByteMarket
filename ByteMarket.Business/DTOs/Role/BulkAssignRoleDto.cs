
namespace ByteMarket.Business.DTOs.Role
{
	public class BulkAssignRoleDto
	{
		public string RoleName { get; set; }
		public List<UserRoleChangeDto> Changes { get; set; }
	}
}
