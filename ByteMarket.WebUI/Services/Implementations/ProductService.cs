using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class ProductService : IProductService
	{
		private readonly IApiService _apiService;
		public ProductService(IApiService apiService) => _apiService = apiService;

		public async Task<bool> AddProductAsync(CreateProductViewModel model)
		{
			var result = await _apiService.PostAsync<CreateProductViewModel>("Products/Add", model);
			return result.Success;
		}

		public async Task<List<ProductListViewModel>> GetProductsForAdminAsync()
		{
			var response = await _apiService.GetAllAsync<ProductListViewModel>("Products/GetAll");

			return response.Success ? response.Data : new List<ProductListViewModel>();
		}
	}
}
