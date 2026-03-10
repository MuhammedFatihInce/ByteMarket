using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Business.DTOs.ProductReview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class ProductReviewController : BaseController
	{
		private readonly IProductReviewService _productReviewService;
		public ProductReviewController(IProductReviewService productReviewService)
		{
			_productReviewService = productReviewService;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Add(CreateProductReviewDto createDto)
		{
			var result = await _productReviewService.CreateProductReviewAsync(createDto);
			return CreateActionResult(result, successStatusCode: 201);
		}

		[HttpGet("{productId}")]
		public async Task<IActionResult> GetAll(string productId)
		{
			var result = await _productReviewService.GetReviewsByProductIdAsync(productId);
			return CreateActionResult(result);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _productReviewService.DeleteProductReviewAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpPut]
		[Authorize]
		public async Task<IActionResult> Update(UpdateProductReviewDto updateDto)
		{
			var result = await _productReviewService.UpdateProductReviewAsync(updateDto);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
