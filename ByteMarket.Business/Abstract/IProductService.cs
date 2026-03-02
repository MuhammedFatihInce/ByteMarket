using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface IProductService
	{
		Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync(string? categoryId = null, string? currentUserId = null);
		Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id, string? currentUserId = null);
		Task<IDataResult<string>> CreateProductAsync(CreateProductDto createProductDto);
		Task<IResult> UpdateProductAsync(UpdateProductDto updateProductDto);
		Task<IResult> DeleteProductAsync(string id);
	}
}
