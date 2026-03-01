
using ByteMarket.Business.Abstract;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace ByteMarket.Business.Concrete
{
	public class MailManager : IMailService
	{
		private readonly IConfiguration _configuration;

		public MailManager(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendMailAsync(string to, string subject, string body, bool isHtml = true)
		{
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress(_configuration["MailSettings:DisplayName"], _configuration["MailSettings:Email"]));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;

			var bodyBuilder = new BodyBuilder { HtmlBody = isHtml ? body : null, TextBody = !isHtml ? body : null };
			email.Body = bodyBuilder.ToMessageBody();

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

			await smtp.AuthenticateAsync(_configuration["MailSettings:Email"], _configuration["MailSettings:Password"]);

			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}

		public async Task SendPasswordResetMailAsync(string to, string resetLink)
		{
			string mailBody = $@"
            <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 20px;'>
                <h2 style='color: #ff6600;'>ByteMarket Şifre Sıfırlama</h2>
                <p>Merhaba, şifrenizi sıfırlama talebinde bulundunuz.</p>
                <p>Aşağıdaki butona tıklayarak yeni şifrenizi belirleyebilirsiniz:</p>
                <a href='{resetLink}' style='background-color: #ff6600; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Şifremi Sıfırla</a>
                <p style='color: #777; font-size: 12px; margin-top: 20px;'>Bu link 3 saat boyunca geçerlidir. Talebi siz yapmadıysanız bu maili görmezden gelebilirsiniz.</p>
            </div>";

			await SendMailAsync(to, "Şifre Sıfırlama Talebi", mailBody);
		}
	}
}
