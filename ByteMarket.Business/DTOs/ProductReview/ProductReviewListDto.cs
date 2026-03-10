

namespace ByteMarket.Business.DTOs.ProductReview
{
	public class ProductReviewListDto
	{
		public string Id { get; set; }
		public string? UserId { get; set; }
		public string UserName { get; set; }
		public string Comment { get; set; }
		public int Rating { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
