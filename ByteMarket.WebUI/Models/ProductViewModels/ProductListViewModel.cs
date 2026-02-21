namespace ByteMarket.WebUI.Models.ProductViewModels
{
	public class ProductListViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string? ImagePath { get; set; }
	}
}
