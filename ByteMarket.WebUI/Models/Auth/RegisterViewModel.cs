using System.ComponentModel.DataAnnotations;

namespace ByteMarket.WebUI.Models.Auth
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Ad Soyad zorunludur.")]
		public string NameSurname { get; set; }

		[Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "E-posta adresi zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre zorunludur.")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
		public string ConfirmPassword { get; set; }
	}
}
