namespace ByteMarket.WebUI.Models.Order
{
	public class OrderItemDetailViewModel:OrderItemViewModel
	{
		public bool IsReviewed { get; set; }
		public string? ReviewId { get; set; }
		public string? Comment { get; set; }
		public int? Rating { get; set; }
	}
}
