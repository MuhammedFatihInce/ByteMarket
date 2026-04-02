using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ByteMarket.WebAPI.Filters
{
	public class CacheAttribute : Attribute, IAsyncActionFilter
	{
		private readonly string? _cacheKey;
		private readonly int _absoluteExpirationMinute;
		private readonly int _slidingExpirationMinute;

		public CacheAttribute(string? cacheKey = null, int absoluteExpirationMinute = 60, int slidingExpirationMinute = 10)
		{
			_cacheKey = cacheKey;
			_absoluteExpirationMinute = absoluteExpirationMinute;
			_slidingExpirationMinute = slidingExpirationMinute;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var cacheService = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

			var actualKey = string.IsNullOrEmpty(_cacheKey)
				? GenerateCacheKeyFromRequest(context.HttpContext.Request)
				: _cacheKey;

			var cachedResponse = await cacheService.GetStringAsync(actualKey);

			if (!string.IsNullOrEmpty(cachedResponse))
			{
				var contentResult = new ContentResult
				{
					Content = cachedResponse,
					ContentType = "application/json",
					StatusCode = 200
				};
				context.Result = contentResult;
				return;
			}

			var executedContext = await next();

			if (executedContext.Result is ObjectResult objectResult &&
			    objectResult.StatusCode >= 200 &&
			    objectResult.StatusCode < 300)
			
			{
				var resultData = JsonSerializer.Serialize(objectResult.Value);

				var options = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_absoluteExpirationMinute),
					SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationMinute)
				};

				await cacheService.SetStringAsync(actualKey, resultData, options);
			}

		}
		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			return $"{request.Path}{request.QueryString}";
		}
	}
}
