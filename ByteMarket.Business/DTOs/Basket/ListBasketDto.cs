
namespace ByteMarket.Business.DTOs.Basket
{
	public class ListBasketDto
	{
		public string Id { get; set; }
		public List<BasketItemDto> BasketItem { get; set; }
		public List<ApplyCouponDto>? Coupons { get; set; }
		public decimal TotalBasePrice { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal FinalTotalPrice { get; set; }

	}
}
