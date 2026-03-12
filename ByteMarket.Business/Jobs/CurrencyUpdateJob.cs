
using ByteMarket.Business.Abstract;
using Quartz;

namespace ByteMarket.Business.Jobs
{
	[DisallowConcurrentExecution]
	public class CurrencyUpdateJob:IJob
	{
		private readonly ICurrencyService _currencyService;

		public CurrencyUpdateJob(ICurrencyService currencyService)
		{
			_currencyService = currencyService;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				await _currencyService.UpdateDatabaseCurrencies();

				
				Console.WriteLine($"TCMB Kurları başarıyla güncellendi: {DateTime.Now}");
			}
			catch (Exception ex)
			{
				
				Console.WriteLine($"Kur güncelleme sırasında hata: {ex.Message}");
			}
		}
	}
}
