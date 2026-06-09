using ByteMarket.WebUI.Models.ProductReview;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IProductReviewService
	{
		Task<ApiDataResponse<string>> AddProductReviewAsync(CreateProductReviewViewModel model);

		Task<ApiDataResponse<List<ProductReviewListViewModel>>> GetReviewsByProductIdAsync(string productId);

		Task<ApiDataResponse<object>> UpdateProductReviewAsync(UpdateProductReviewViewModel model);

		Task<ApiDataResponse<object>> DeleteProductReviewAsync(string id);
		Task<ApiDataResponse<bool>> HasUserReviewedProductAsync(string productId);
	}
}
