namespace ByteMarket.WebUI.Models.Order
{
	public class SingleOrderViewModel
	{
		public string Id { get; set; }
		public string OrderCode { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public IEnumerable<OrderItemViewModel> BasketItems { get; set; }
	}
}
