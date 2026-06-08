namespace ByteMarket.WebUI.Models.Order
{
	public class OrderItemViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? ImagePath { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
