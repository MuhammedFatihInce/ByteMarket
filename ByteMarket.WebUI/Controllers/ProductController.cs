using ByteMarket.WebUI.Models.ProductReview;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ByteMarket.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly IApiService _apiService;
		private readonly ICategoryService _categoryService;
		private readonly IProductReviewService _productReviewService;

		public ProductController(IProductService productService, IApiService apiService, ICategoryService categoryService, IProductReviewService productReviewService)
		{
			_productService = productService;
			_apiService = apiService;
			_categoryService = categoryService;
			_productReviewService = productReviewService;
		}

		[HttpGet]
		public async Task<IActionResult> Detail(string id)
		{
			var product = await _productService.GetProductDetailsAsync(id);

			var review = await _productReviewService.GetReviewsByProductIdAsync(id);

			if (!product.Success) return RedirectToAction("Index", "Home");

			return View((product.Data, review.Data));
		}

		[HttpPost("Product/AddReview")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddReview([FromBody] CreateProductReviewViewModel model)
		{
			var result = await _productReviewService.AddProductReviewAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpPut("Product/UpdateReview")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateReview([FromBody] UpdateProductReviewViewModel model)
		{
			var result = await _productReviewService.UpdateProductReviewAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete("Product/DeleteReview/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteReview(string id)
		{
			var result = await _productReviewService.DeleteProductReviewAsync(id);

			return Json(new { success = result.Success, message = result.Message });
		}
	}
}
