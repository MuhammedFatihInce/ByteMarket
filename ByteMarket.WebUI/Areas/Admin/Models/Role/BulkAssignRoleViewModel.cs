namespace ByteMarket.WebUI.Areas.Admin.Models.Role
{
	public class BulkAssignRoleViewModel
	{
		public string RoleName { get; set; }
		public List<UserRoleChangeViewModel> Changes { get; set; }
	}
}
