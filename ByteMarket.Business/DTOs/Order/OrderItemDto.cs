
namespace ByteMarket.Business.DTOs.Order
{
	public class OrderItemDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? ImagePath { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
