using ByteMarket.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class ProductImagesController : BaseController
	{
		private readonly IProductImageService _productImageService;

		public ProductImagesController(IProductImageService productImageService)
		{
			_productImageService = productImageService;
		}

		[HttpPost("Upload/{id}")]
		public async Task<IActionResult> Upload(string id, IFormFileCollection files)
		{
			var result = await _productImageService.AddImagesAsync(id, files);
			return CreateActionResult(result, successStatusCode:201);
		}

		[HttpDelete("Delete/{id}")] 
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _productImageService.DeleteImageAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
