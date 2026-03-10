
namespace ByteMarket.Business.DTOs.ProductReview
{
	public class CreateProductReviewDto
	{
		public string Comment { get; set; }
		public string ProductId { get; set; }
		public int Rating { get; set; }
	}
}
