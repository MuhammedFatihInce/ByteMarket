
namespace ByteMarket.Business.DTOs.Currency
{
	public class CurrencyDto
	{
		public string Code { get; set; }        
		public string Name { get; set; }        
		public decimal BuyingRate { get; set; } 
		public decimal SellingRate { get; set; } 
	}
}
