using System.ComponentModel.DataAnnotations;

namespace ByteMarket.WebUI.Models.Auth
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "E-posta adresi zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
		public string Email { get; set; }
	}
}
