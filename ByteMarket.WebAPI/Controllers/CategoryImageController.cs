
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	[Authorize(Policy = AuthorizePolicies.AdminOnly)]
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _categoryImageService.DeleteImageAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
