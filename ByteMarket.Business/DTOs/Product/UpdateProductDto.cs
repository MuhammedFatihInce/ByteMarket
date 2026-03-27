
namespace ByteMarket.Business.DTOs.Product
{
	public class UpdateProductDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public List<string> CategoryIds { get; set; }
		public List<string> OrderedImageIds { get; set; }
		public byte[] RowVersion { get; set; }

	}
}
