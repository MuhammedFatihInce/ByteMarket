

namespace ByteMarket.WebUI.Models.ProductReview
{
	public class ProductReviewListViewModel
	{
		public string Id { get; set; }
		public string? UserId { get; set; }
		public string UserName { get; set; }
		public string Comment { get; set; }
		public int Rating { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
