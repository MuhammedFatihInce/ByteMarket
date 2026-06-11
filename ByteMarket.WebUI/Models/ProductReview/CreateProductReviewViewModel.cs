
namespace ByteMarket.WebUI.Models.ProductReview
{
	public class CreateProductReviewViewModel
	{
		public string Comment { get; set; }
		public int Rating { get; set; }
		public string ProductId { get; set; }
		public string OrderId { get; set; }
	}
}
