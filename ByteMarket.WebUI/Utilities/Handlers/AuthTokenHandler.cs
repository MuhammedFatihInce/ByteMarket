using ByteMarket.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace ByteMarket.WebUI.Utilities.Handlers
{
	public class AuthTokenHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IServiceProvider _serviceProvider;

		public AuthTokenHandler(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
		{
			_httpContextAccessor = httpContextAccessor;
			_serviceProvider = serviceProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{

			if (request.RequestUri.PathAndQuery.Contains("auth/login") ||
			    request.RequestUri.PathAndQuery.Contains("auth/refresh-token-login"))
			{
				return await base.SendAsync(request, cancellationToken);
			}

			var jwt = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

			if (!string.IsNullOrEmpty(jwt))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
			}

			var response = await base.SendAsync(request, cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				using var scope = _serviceProvider.CreateScope();
				var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

				var isRefreshed = await accountService.RefreshTokenAsync();

				if (isRefreshed)
				{
					var newJwt = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];

					response.Dispose();
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwt);

					return await base.SendAsync(request, cancellationToken);
				}
			}

			return response;
		}
	}
}
