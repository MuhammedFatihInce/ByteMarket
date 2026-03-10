namespace ByteMarket.WebUI.Areas.Admin.Models.User
{
	public class GetAllUsersByFilterViewModel
	{
		public string Id { get; set; }
		public string NameSurname { get; set; }
		public string Email { get; set; }
		public List<string> RoleIds { get; set; }
	}
}
