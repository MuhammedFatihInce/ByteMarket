using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll([FromQuery] string? categoryId)
		{
			var result = await _productService.GetAllProductsAsync(categoryId);

			return CreateActionResult(result);
		}

		[HttpGet("GetById/{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await _productService.GetProductByIdAsync(id);

			return CreateActionResult(result);
		}

		[HttpPost("Add")]
		public async Task<IActionResult> Add(CreateProductDto createProductDto)
		{
			var result = await _productService.CreateProductAsync(createProductDto);

			return CreateActionResult(result, 201);
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateProductDto updateProductDto)
		{
			var result = await _productService.UpdateProductAsync(updateProductDto);
			return CreateActionResult(result);
		}

		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _productService.DeleteProductAsync(id);
			return CreateActionResult(result);
		}
	}
}
