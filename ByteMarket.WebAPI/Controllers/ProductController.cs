using ByteMarket.Business.Abstract;
using ByteMarket.Entities.Constants;
using ByteMarket.Business.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ByteMarket.WebAPI.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] string? categoryId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _productService.GetAllProductsAsync(categoryId, currentUserId);

			return CreateActionResult(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _productService.GetProductByIdAsync(id, currentUserId);

			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpPost]
		[Authorize(Policy = AuthorizePolicies.FullProductManagement)]
		public async Task<IActionResult> Add(CreateProductDto createProductDto)
		{
			var result = await _productService.CreateProductAsync(createProductDto);

			return CreateActionResult(result, successStatusCode:201);
		}

		[HttpPut]
		[Authorize(Policy = AuthorizePolicies.FullProductManagement)]
		public async Task<IActionResult> Update([FromBody] UpdateProductDto updateProductDto)
		{
			var result = await _productService.UpdateProductAsync(updateProductDto);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpDelete("{id}")]
		[Authorize(Policy = AuthorizePolicies.FullProductManagement)]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _productService.DeleteProductAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpGet("GetAllProductsByFilter")]
		public async Task<IActionResult> GetAllProductsByFilter([FromQuery]string q)
		{
			var result = await _productService.GetAllProductsByFilterAsync(q);
			return CreateActionResult(result);
		}
	}
}
