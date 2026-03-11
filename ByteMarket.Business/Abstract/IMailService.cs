
using ByteMarket.Business.DTOs.Order;

namespace ByteMarket.Business.Abstract
{
	public interface IMailService
	{
		Task SendMailAsync(string to, string subject, string body, bool isHtml = true);
		Task SendPasswordResetMailAsync(string to, string resetLink);
		Task SendInvoiceMailAsync(string to, SingleOrderDto orderDto);
	}
}
