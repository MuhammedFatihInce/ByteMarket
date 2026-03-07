
namespace ByteMarket.Business.DTOs.Coupon
{
	public class CreateCouponDto
	{
		public string Code { get; set; }
		public int Target { get; set; }
		public decimal DiscountValue { get; set; }
		public bool IsPercentage { get; set; }
		public List<string>? ProductIds { get; set; }
	}
}
