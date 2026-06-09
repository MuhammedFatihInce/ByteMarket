
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PuppeteerSharp;
using PuppeteerSharp.Media;
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

		private MimeMessage CreateBaseMessage(string to, string subject)
		{
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress(_configuration["MailSettings:DisplayName"], _configuration["MailSettings:Email"]));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;
			return email;
		}
		private async Task SendEmailInternalAsync(MimeMessage email)
		{
			using var smtp = new SmtpClient();
			try
			{
				await smtp.ConnectAsync(_configuration["MailSettings:Host"],
					int.Parse(_configuration["MailSettings:Port"]),
					MailKit.Security.SecureSocketOptions.StartTls);

				await smtp.AuthenticateAsync(_configuration["MailSettings:Email"],
					_configuration["MailSettings:Password"]);

				await smtp.SendAsync(email);
			}
			finally
			{
				await smtp.DisconnectAsync(true);
			}
		}

		public async Task SendMailAsync(string to, string subject, string body, bool isHtml = true)
		{

			var email = CreateBaseMessage(to, subject);

			var bodyBuilder = new BodyBuilder { HtmlBody = isHtml ? body : null, TextBody = !isHtml ? body : null };
			email.Body = bodyBuilder.ToMessageBody();

			await SendEmailInternalAsync(email);
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

		public async Task SendInvoiceMailAsync(string to, InvoiceOrderDto orderDto)
		{
			try
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


				string mailHtmlContent = GetInvoiceHtmlTemplate(orderDto, itemsHtml.ToString());

				var pdfBytes = await GeneratePdfBytes(mailHtmlContent);

				
				var email = CreateBaseMessage(to, $"Sipariş Faturası - #{orderDto.OrderCode}");

				var bodyBuilder = new BodyBuilder
				{
					HtmlBody = $@"<p>Sayın Müşterimiz,</p>
                             <p>#{orderDto.OrderCode} nolu siparişinize ait fatura detayları aşağıdadır ve ekte PDF olarak sunulmuştur.</p>
                             <hr/>" + mailHtmlContent 
				};

				
				bodyBuilder.Attachments.Add($"Fatura_{orderDto.OrderCode}.pdf", pdfBytes);
				email.Body = bodyBuilder.ToMessageBody();

				await SendEmailInternalAsync(email);
			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}

			
		}

		private async Task<byte[]> GeneratePdfBytes(string htmlContent)
		{
			
			var browserFetcher = new BrowserFetcher();
			await browserFetcher.DownloadAsync();

			
			using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

			using var page = await browser.NewPageAsync();
			await page.SetContentAsync(htmlContent);

			
			return await page.PdfDataAsync(new PdfOptions
			{
				Format = PaperFormat.A4,
				PrintBackground = true, 
				MarginOptions = new MarginOptions
				{
					Top = "10mm",
					Bottom = "10mm",
					Left = "10mm",
					Right = "10mm"
				}
			});
		}

		private string GetInvoiceHtmlTemplate(InvoiceOrderDto orderDto, string itemsHtml)
		{
			return $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #0d6efd;'>ByteMarket Faturası</h2>
                    <p><strong>Sipariş No:</strong> #{orderDto.OrderCode}<br>
                    <strong>Tarih:</strong> {orderDto.CreatedDate:dd.MM.yyyy}</p>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <thead style='background-color: #f8f9fa;'>
                            <tr>
                                <th style='text-align:left; padding:10px;'>Ürün</th>
                                <th style='text-align:center;'>Adet</th>
                                <th style='text-align:right;'>Fiyat</th>
                                <th style='text-align:right; padding:10px;'>Toplam</th>
                            </tr>
                        </thead>
                        <tbody>{itemsHtml}</tbody>
                    </table>
                    <div style='text-align: right; margin-top: 20px;'>
                        <p>Ara Toplam: {orderDto.TotalBasePrice:C2}</p>
                        <p style='color:red;'>İndirim: -{orderDto.DiscountAmount:C2}</p>
                        <h3 style='color: #0d6efd;'>Genel Toplam: {orderDto.FinalTotalPrice:C2}</h3>
                    </div>
                </div>";
		}
	}
}
