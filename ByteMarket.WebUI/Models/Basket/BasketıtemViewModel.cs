namespace ByteMarket.WebUI.Models.Basket
{
	public class BasketItemViewModel
	{
		public string BasketItemId { get; set; }
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal Total { get; set; }
		public string? ImagePath { get; set; }
	}
}
