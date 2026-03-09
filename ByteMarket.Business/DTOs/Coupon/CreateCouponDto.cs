
namespace ByteMarket.Business.DTOs.Coupon
{
	public class CreateCouponDto
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public int Target { get; set; }
		public decimal DiscountValue { get; set; }
		public bool IsPercentage { get; set; }
		public DateTime ExpireTime { get; set; }
		public bool IsStackable { get; set; }
		public int UsageLimitPerUser { get; set; }
		public List<string>? ProductIds { get; set; }
	}
}
