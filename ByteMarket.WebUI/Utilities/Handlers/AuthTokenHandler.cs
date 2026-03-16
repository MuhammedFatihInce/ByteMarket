using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

			var context = _httpContextAccessor.HttpContext;
			var jwt = context?.Request.Cookies["jwt"];

			if (!string.IsNullOrEmpty(jwt))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
			}

			var response = await base.SendAsync(request, cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				using var scope = _serviceProvider.CreateScope();
				var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

				var (isRefreshed, newJwt) = await accountService.RefreshTokenAsync();

				if (isRefreshed)
				{
					response.Dispose();
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwt);

					return await base.SendAsync(request, cancellationToken);
				}
				else
				{
					if (context != null)
					{
						await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
						context.Response.Cookies.Delete("jwt");
						context.Response.Cookies.Delete("refreshToken");
					}

					return response;
				}
				
			}

			return response;
		}
	}
}
