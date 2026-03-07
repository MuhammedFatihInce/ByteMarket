namespace ByteMarket.WebUI.Areas.Admin.Models.Coupon
{
	public class UpdateCouponViewModel
	{
		public string Id { get; set; }
		public string Code { get; set; }
		public int Target { get; set; }
		public decimal DiscountValue { get; set; }
		public bool IsPercentage { get; set; }
		public List<string> ProductIds { get; set; }
	}
}
