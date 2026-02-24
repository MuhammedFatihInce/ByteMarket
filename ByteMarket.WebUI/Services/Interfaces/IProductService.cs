using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IProductService
	{
		Task<ApiDataResponse<string>> AddProductAsync(CreateProductViewModel model);
		Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForAdminAsync();
		Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForCustomerAsync();
		Task<ApiDataResponse<object>> UpdateProductWithImagesAsync(UpdateProductViewModel model);
		Task<ApiDataResponse<SingleProductViewModel>> GetProductDetailsAsync(string id);
	}
}
