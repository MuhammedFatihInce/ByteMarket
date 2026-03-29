using ByteMarket.WebAPI.Hubs;
using ByteMarket.WebAPI.SignalRServices.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace ByteMarket.WebAPI.SignalRServices.Concrete
{
	public class StockNotificationManager : IStockNotificationService
	{
		private readonly IHubContext<StockHub> _hubContext;

		public StockNotificationManager(IHubContext<StockHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task SendStockUpdateAsync(string productId, int newStock)
		{
			await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", productId, newStock);
		}
	}
}
