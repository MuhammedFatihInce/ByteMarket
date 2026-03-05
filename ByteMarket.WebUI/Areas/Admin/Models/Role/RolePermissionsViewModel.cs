namespace ByteMarket.WebUI.Areas.Admin.Models.Role
{
	public class RolePermissionsViewModel
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public List<PermissionCheckViewModel> Permissions { get; set; } = new();
	}
}
