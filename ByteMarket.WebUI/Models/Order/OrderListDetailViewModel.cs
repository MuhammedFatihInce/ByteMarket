namespace ByteMarket.WebUI.Models.Order
{
	public class OrderListDetailViewModel
	{
		public string Id { get; set; }
		public string OrderCode { get; set; }
		public string UserName { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
