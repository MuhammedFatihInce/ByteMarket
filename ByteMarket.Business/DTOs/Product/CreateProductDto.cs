
namespace ByteMarket.Business.DTOs.Product
{
	public class CreateProductDto
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public List<string> CategoryIds { get; set; }
	}
}
