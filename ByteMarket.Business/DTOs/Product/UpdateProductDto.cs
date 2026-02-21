
namespace ByteMarket.Business.DTOs.Product
{
	public class UpdateProductDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
	}
}
