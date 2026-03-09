namespace ByteMarket.WebUI.Models.Order
{
	public class CreateOrderViewModel
	{
		public string BasketId { get; set; }

		public string Address { get; set; }

		public string Description { get; set; }
		public decimal TotalBasePrice { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal FinalTotalPrice { get; set; }
	}
}
