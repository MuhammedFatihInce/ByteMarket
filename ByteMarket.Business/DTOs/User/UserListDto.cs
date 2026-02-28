
namespace ByteMarket.Business.DTOs.User
{
	public class UserListDto
	{
		public string Id { get; set; }
		public string NameSurname { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public IList<string> Roles { get; set; } 
	}
}
