using ByteMarket.Business.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace ByteMarket.WebAPI.Controllers
{
	public class EditorController : BaseController
	{
		private readonly IEditorService _editorService;
		public EditorController(IEditorService editorService)
		{
			_editorService = editorService;
		}

		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			var result = await _editorService.UploadImageAsync(file, "editor-images");
			return CreateActionResult(result, successStatusCode: 201);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery] string url)
		{
			var result = await _editorService.DeleteImageAsync(url, "editor-images");
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
