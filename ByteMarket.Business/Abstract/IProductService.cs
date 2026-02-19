
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IProductService
	{
		Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync();
		Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id);
		Task<IResult> CreateProductAsync(CreateProductDto createProductDto);

		// İleride buraya StockUpdate veya PriceChange gibi iş mantıkları gelecek
	}
}
