namespace ByteMarket.WebUI.Models.User
{
	public class UserListViewModel
	{
		public string Id { get; set; }
		public string NameSurname { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public List<string> Roles { get; set; }
	}
}
