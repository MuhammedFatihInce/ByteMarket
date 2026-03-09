
namespace ByteMarket.WebUI.Areas.Admin.Models.Coupon
{
	public class CreateCouponViewModel
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
