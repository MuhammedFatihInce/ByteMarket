using ByteMarket.WebUI.Models.ProductReview;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class ProductReviewService : IProductReviewService
	{
		private readonly IApiService _apiService;
		public ProductReviewService(IApiService apiService) => _apiService = apiService;

		public async Task<ApiDataResponse<string>> AddProductReviewAsync(CreateProductReviewViewModel model)
		{
			return await _apiService.PostAsync<string>("ProductReview", model);
		}

		public async Task<ApiDataResponse<List<ProductReviewListViewModel>>> GetReviewsByProductIdAsync(string productId)
		{
			return await _apiService.GetAllAsync<ProductReviewListViewModel>($"ProductReview/{productId}");
		}

		public async Task<ApiDataResponse<object>> UpdateProductReviewAsync(UpdateProductReviewViewModel model)
		{
			return await _apiService.PutAsync<object>("ProductReview", model);
		}

		public async Task<ApiDataResponse<object>> DeleteProductReviewAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("ProductReview", id);
		}
	}
}
