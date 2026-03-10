
namespace ByteMarket.Business.DTOs.User
{
	public class GetAllUsersByFilterDto
	{
		public string Id { get; set; }
		public string NameSurname { get; set; }
		public string Email { get; set; }
		public List<string> RoleIds { get; set; }
	}
}
