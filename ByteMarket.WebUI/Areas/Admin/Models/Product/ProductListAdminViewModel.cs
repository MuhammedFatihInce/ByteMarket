namespace ByteMarket.WebUI.Areas.Admin.Models.Product
{
	public class ProductListAdminViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public List<string>? ImagePath { get; set; }
		public string? CategoryName { get; set; }
	}
}
