using ByteMarket.Business.Utilities.Results;
using System;
using System.Net;
using System.Text.Json;

namespace ByteMarket.WebAPI.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Sistemde beklenmeyen bir kritik hata oluştu! İstek Yolu: {Method} {Path}",
					httpContext.Request.Method,
					httpContext.Request.Path);

				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

			

			var response = new ErrorDataResult<string>(
				 "Sunucu tarafında beklenmedik bir hata oluştu."
			);

			var json = JsonSerializer.Serialize(response);
			return context.Response.WriteAsync(json);
		}
	}
}
