using ByteMarket.WebUI.Areas.Admin.Models.Product;
using ByteMarket.WebUI.Models.Product;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IProductService
	{
		Task<ApiDataResponse<string>> AddProductAsync(CreateProductViewModel model);
		Task<ApiDataResponse<List<ProductListAdminViewModel>>> GetProductsForAdminAsync(string? categoryId = null);
		Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForCustomerAsync(string? categoryId = null);
		Task<ApiDataResponse<object>> UpdateProductWithImagesAsync(UpdateProductViewModel model);
		Task<ApiDataResponse<SingleProductViewModel>> GetProductDetailsAsync(string id);
		Task<ApiDataResponse<object>> DeleteProductAsync(string id);
		Task<ApiDataResponse<object>> DeleteProductImageAsync(string id);
		Task<ApiDataResponse<List<GetAllProductByFilterViewModel>>> GetAllProductByFilterAsync(string q);
	}
}
