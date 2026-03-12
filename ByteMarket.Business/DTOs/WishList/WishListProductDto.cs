
namespace ByteMarket.Business.DTOs.WishList
{
	public class WishListProduct
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public List<string>? ImagePath { get; set; }
		public string? CategoryName { get; set; }
	}
}
