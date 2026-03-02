namespace ByteMarket.WebUI.Models.wishList
{
	public class WishListProductViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string? ImagePath { get; set; }
		public string? CategoryName { get; set; }
	}
}
