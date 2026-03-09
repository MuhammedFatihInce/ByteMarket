
namespace ByteMarket.Business.DTOs.Order
{
	public class SingleOrderDto
	{
		public string Id { get; set; }
		public string OrderCode { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public IEnumerable<OrderItemDto> BasketItems { get; set; }
		public decimal TotalBasePrice { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal FinalTotalPrice { get; set; }
	}
}
