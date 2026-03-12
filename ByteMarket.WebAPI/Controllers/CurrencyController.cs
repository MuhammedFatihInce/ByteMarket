using ByteMarket.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	
	public class CurrencyController : BaseController
	{
		private readonly ICurrencyService _currencyService;
		public CurrencyController(ICurrencyService currencyService)
		{
			_currencyService = currencyService;
		}


		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _currencyService.GetAllCurrenciesFromDbAsync();
			return CreateActionResult(result);
		}

		[HttpGet("{code}")]
		public async Task<IActionResult> GetbyCode(string code)
		{
			var result = await _currencyService.GetCurrencyByCodeAsync(code);
			return CreateActionResult(result);
		}
	}
}
