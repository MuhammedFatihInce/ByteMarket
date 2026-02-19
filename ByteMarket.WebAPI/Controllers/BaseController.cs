using ByteMarket.Business.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		[NonAction]
		public IActionResult CreateActionResult<T>(IDataResult<T> result, int successStatusCode = 200)
		{
			if (result.Success)
				return StatusCode(successStatusCode, result);

			return result.Data == null
				? NotFound(result)
				: BadRequest(result);
		}

		[NonAction]
		public IActionResult CreateActionResult(IResult result, int successStatusCode = 200)
		{
			if (result.Success)
				return StatusCode(successStatusCode, result);

			return BadRequest(result);
		}
	}
}
