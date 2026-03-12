

using ByteMarket.Entities.Common;

namespace ByteMarket.Entities.Concrete
{
	public class Currency: BaseEntity
	{
		public string Code { get; set; } 
		public string Name { get; set; } 
		public decimal BuyingRate { get; set; }
		public decimal SellingRate { get; set; }
	}
}
