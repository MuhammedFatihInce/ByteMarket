using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IProductService
	{
		Task<ApiDataResponse<string>> AddProductAsync(CreateProductViewModel model);
		Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForAdminAsync(string? categoryId = null);
		Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForCustomerAsync(string? categoryId = null);
		Task<ApiDataResponse<object>> UpdateProductWithImagesAsync(UpdateProductViewModel model);
		Task<ApiDataResponse<SingleProductViewModel>> GetProductDetailsAsync(string id);
	}
}
