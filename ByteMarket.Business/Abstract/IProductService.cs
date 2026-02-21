
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using Microsoft.AspNetCore.Http;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface IProductService
	{
		Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync();
		Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id);
		Task<IDataResult<string>> CreateProductAsync(CreateProductDto createProductDto);
		Task<IResult> AddProductImagesAsync(string productId, IFormFileCollection files);
		Task<IResult> UpdateProductAsync(UpdateProductDto updateProductDto);
		Task<IResult> DeleteProductImageAsync(string imageId);
		Task<IResult> DeleteProductAsync(string id);
	}
}
