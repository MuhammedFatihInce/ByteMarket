
using ByteMarket.Business.DTOs.ProductReview;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IProductReviewService
	{
		Task<IResult> CreateProductReviewAsync(CreateProductReviewDto createProductReviewDto);
		Task<IDataResult<List<ProductReviewListDto>>> GetReviewsByProductIdAsync(string productId);
		Task<IResult> UpdateProductReviewAsync(UpdateProductReviewDto updateDto);
		Task<IResult> DeleteProductReviewAsync(string reviewId);
		Task<IDataResult<bool>> HasUserReviewedProductAsync(string userId, string productId, string orderId);
	}
}
