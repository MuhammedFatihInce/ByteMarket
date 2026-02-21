
namespace ByteMarket.Business.DTOs.Product
{
	public class ListProductDto
	{
		public string Id { get; set; } 
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string? ImagePath { get; set; }
	}
}
