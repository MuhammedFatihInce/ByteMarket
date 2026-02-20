using ByteMarket.WebUI.Models.ProductViewModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IProductService
	{
		Task<bool> AddProductAsync(CreateProductViewModel model);
		Task<List<ProductListViewModel>> GetProductsForAdminAsync();
	}
}
