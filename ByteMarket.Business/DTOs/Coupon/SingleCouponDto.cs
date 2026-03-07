
namespace ByteMarket.Business.DTOs.Coupon
{
	public class SingleCouponDto
	{
		public string Id { get; set; }
		public string Code { get; set; }
		public int Target { get; set; }
		public decimal DiscountValue { get; set; }
		public bool IsPercentage { get; set; }
		public List<CouponProductDto>? Products { get; set; }
	}
}
