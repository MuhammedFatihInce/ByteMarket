namespace ByteMarket.WebAPI.SignalRServices.Abstract
{
	public interface IStockNotificationService
	{
		Task SendStockUpdateAsync(string productId, int newStock);
	}
}
