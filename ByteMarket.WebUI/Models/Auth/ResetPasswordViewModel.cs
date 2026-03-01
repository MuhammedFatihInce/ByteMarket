using System.ComponentModel.DataAnnotations;

namespace ByteMarket.WebUI.Models.Auth
{
	public class ResetPasswordViewModel
	{
		public string Email { get; set; }
		public string Token { get; set; } 

		[Required(ErrorMessage = "Yeni şifre zorunludur.")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
		public string ConfirmPassword { get; set; }
	}
}
