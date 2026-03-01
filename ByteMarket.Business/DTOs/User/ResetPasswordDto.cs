
namespace ByteMarket.Business.DTOs.User
{
	public class ResetPasswordDto
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string NewPassword { get; set; }
	}
}
