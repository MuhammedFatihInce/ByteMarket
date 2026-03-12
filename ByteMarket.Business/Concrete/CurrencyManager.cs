
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Currency;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Currency;
using ByteMarket.Entities.Concrete;
using System.Globalization;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class CurrencyManager:ICurrencyService
	{
		private readonly ICurrencyReadRepository _currencyReadRepository;
		private readonly ICurrencyWriteRepository _currencyWriteRepository;
		public CurrencyManager(ICurrencyReadRepository currencyReadRepository ,ICurrencyWriteRepository currencyWriteRepository)
		{
			_currencyReadRepository = currencyReadRepository;
			_currencyWriteRepository = currencyWriteRepository;
		}

		private async Task<List<CurrencyDto>> GetTCMBCurrencies()
		{
			List<CurrencyDto> currencies = new List<CurrencyDto>();
			string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					var response = await client.GetStringAsync(url);
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);

					XmlNodeList nodes = xmlDoc.SelectNodes("//Currency");

					foreach (XmlNode node in nodes)
					{
						string code = node.Attributes["CurrencyCode"]?.Value;

						string forexBuying = node["ForexBuying"]?.InnerText;
						string forexSelling = node["ForexSelling"]?.InnerText;

						if (!string.IsNullOrEmpty(forexBuying) && !string.IsNullOrEmpty(forexSelling))
						{
							currencies.Add(new CurrencyDto
							{
								Code = code,
								Name = node["Isim"]?.InnerText,
								BuyingRate = decimal.Parse(forexBuying, CultureInfo.InvariantCulture),
								SellingRate = decimal.Parse(forexSelling, CultureInfo.InvariantCulture)
							});
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Hata oluştu: " + ex.Message);
				}
			}

			return currencies;
		}

		public async Task UpdateDatabaseCurrencies()
		{
			var xmlCurrencies = await GetTCMBCurrencies();
			if (xmlCurrencies == null || !xmlCurrencies.Any()) return;

			var existingCurrencies = _currencyReadRepository.GetAll()
				.ToDictionary(x => x.Code);

			foreach (var item in xmlCurrencies)
			{

				if (existingCurrencies.TryGetValue(item.Code, out var existingCurrency))
				{
					existingCurrency.BuyingRate = item.BuyingRate;
					existingCurrency.SellingRate = item.SellingRate;
					existingCurrency.Name = item.Name;

				}
				else
				{

					await _currencyWriteRepository.AddAsync(new Currency
					{
						Code = item.Code,
						Name = item.Name,
						BuyingRate = item.BuyingRate,
						SellingRate = item.SellingRate,
					});
				}
			}
			await _currencyWriteRepository.SaveAsync();
		}


		public async Task<IDataResult<List<CurrencyDto>>> GetAllCurrenciesFromDbAsync()
		{
			var currencyDto= await _currencyReadRepository.GetAll(false)
				.Select(c => new CurrencyDto
				{
					Code = c.Code,
					Name = c.Name,
					BuyingRate = c.BuyingRate,
					SellingRate = c.SellingRate
				}).ToListAsync();


			return new SuccessDataResult<List<CurrencyDto>>(currencyDto);
		}

		public async Task<IDataResult<CurrencyDto>> GetCurrencyByCodeAsync(string code)
		{
			var currency = await _currencyReadRepository.GetSingleAsync(x => x.Code == code, false);
			if (currency == null) return new ErrorDataResult<CurrencyDto>();

			var currencyDto = new CurrencyDto
							{
								Code = currency.Code,
								Name = currency.Name,
								BuyingRate = currency.BuyingRate,
								SellingRate = currency.SellingRate
							};

			return new SuccessDataResult<CurrencyDto>(currencyDto);
		}


	}
}
