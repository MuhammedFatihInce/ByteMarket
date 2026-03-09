namespace ByteMarket.WebUI.Models.Basket
{
	public class ListBasketViewModel
	{
		public string Id { get; set; }
		public List<BasketItemViewModel> BasketItem { get; set; }
		public List<ApplyCouponViewModel>? Coupons { get; set; }
		public decimal TotalBasePrice { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal FinalTotalPrice { get; set; }
	}
}
