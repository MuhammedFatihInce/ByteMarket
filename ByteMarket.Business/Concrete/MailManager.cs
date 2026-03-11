
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Text;

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

		public async Task SendInvoiceMailAsync(string to, SingleOrderDto orderDto)
		{

			StringBuilder itemsHtml = new StringBuilder();
			foreach (var item in orderDto.BasketItems)
			{
				itemsHtml.Append($@"
	            <tr>
	                <td style='padding: 8px; border-bottom: 1px solid #eee;'>{item.Name}</td>
	                <td style='padding: 8px; border-bottom: 1px solid #eee; text-align: center;'>{item.Quantity}</td>
	                <td style='padding: 8px; border-bottom: 1px solid #eee; text-align: right;'>{item.Price:C2}</td>
	                <td style='padding: 8px; border-bottom: 1px solid #eee; text-align: right;'>{(item.Price * item.Quantity):C2}</td>
	            </tr>");
			}


			string mailBody = $@"
					<div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto; border: 1px solid #eee; padding: 20px;'>
						<div style='display: flex; justify-content: space-between; margin-bottom: 20px;'>
							<div style='width: 50%;'>
								<h4 style='color: #0d6efd; margin-bottom: 5px;'>ByteMarket</h4>
								<p style='font-size: 12px; color: #666;'>{orderDto.Address}</p>
							</div>
							<div style='width: 50%; text-align: right;'>
								<h6 style='margin: 0;'>Fatura Tarihi: {orderDto.CreatedDate:dd.MM.yyyy}</h6>
								<h6 style='margin: 0;'>Sipariş No: #{orderDto.OrderCode}</h6>
							</div>
						</div>

						<table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
							<thead>
								<tr style='background-color: #f8f9fa;'>
									<th style='text-align: left; padding: 10px;'>Ürün</th>
									<th style='text-align: center; padding: 10px;'>Adet</th>
									<th style='text-align: right; padding: 10px;'>Fiyat</th>
									<th style='text-align: right; padding: 10px;'>Toplam</th>
								</tr>
							</thead>
							<tbody>
								{itemsHtml}
							</tbody>
						</table>

						<hr style='border: 0; border-top: 1px solid #eee;' />

						<div style='display: flex; justify-content: flex-end;'>
							<div style='width: 40%; float: right;'>
								<div style='display: flex; justify-content: space-between; margin-bottom: 5px;'>
									<span>Ara Toplam:</span>
									<span>{orderDto.TotalBasePrice:C2}</span>
								</div>
								<div style='display: flex; justify-content: space-between; margin-bottom: 5px; color: #dc3545;'>
									<span>İndirim:</span>
									<span>-{orderDto.DiscountAmount:C2}</span>
								</div>
								<div style='display: flex; justify-content: space-between; border-top: 2px solid #eee; pt-2; font-weight: bold; color: #0d6efd;'>
									<span>Genel Toplam:</span>
									<span>{orderDto.FinalTotalPrice:C2}</span>
								</div>
							</div>
							<div style='clear: both;'></div>
						</div>
		            
						<div style='margin-top: 30px; text-align: center; font-size: 12px; color: #999;'>
							Bizi tercih ettiğiniz için teşekkür ederiz!
						</div>
					</div>";

			await SendMailAsync(to, $"Sipariş Faturası - #{orderDto.OrderCode}", mailBody);
		}
	}
}
