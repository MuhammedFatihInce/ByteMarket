
namespace ByteMarket.Business.DTOs.Basket
{
	public class BasketItemDto
	{
		public string BasketItemId { get; set; }
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal Total { get; set; }
		public decimal DiscountAmount { get; set; }
		public string? ImagePath { get; set; }
	}
}
