using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ByteMarket.WebAPI.Filters
{
	public class CacheInvalidateAttribute : Attribute, IAsyncActionFilter
	{
		private readonly string _cacheKey;

		public CacheInvalidateAttribute(string cacheKey)
		{
			_cacheKey = cacheKey;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var executedContext = await next();

			if (executedContext.Result is ObjectResult obj && obj.StatusCode >= 200 && obj.StatusCode < 300)
			{
				var cacheService = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

				await cacheService.RemoveAsync(_cacheKey);
			}
		}
	}
}
