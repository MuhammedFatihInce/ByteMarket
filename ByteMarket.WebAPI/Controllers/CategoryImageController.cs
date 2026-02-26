
using ByteMarket.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class CategoryImageController : BaseController
	{
		private readonly ICategoryImageFileService _categoryImageService;

		public CategoryImageController(ICategoryImageFileService categoryImageFileService)
		{
			_categoryImageService = categoryImageFileService;
		}

		[HttpPost("Upload/{id}")]
		public async Task<IActionResult> Upload(string id, IFormFile file)
		{
			var result = await _categoryImageService.AddImageAsync(id, file);
			return CreateActionResult(result, successStatusCode: 201);
		}

		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _categoryImageService.DeleteImageAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
