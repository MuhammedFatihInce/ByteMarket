
using ByteMarket.Entities.Common;
using ByteMarket.Entities.Enums;

namespace ByteMarket.Entities.Concrete
{
	public class Coupon:BaseEntity
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public DiscountTarget Target { get; set; } 
		public decimal DiscountValue { get; set; } 
		public bool IsPercentage { get; set; }
		public DateTime ExpireTime { get; set; }
		public bool IsStackable { get; set; }
		public int UsageLimitPerUser { get; set; }
		public ICollection<Product> Products { get; set; } 
		public ICollection<Basket> Baskets { get; set; }
	}
}
