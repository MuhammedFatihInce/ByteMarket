using System.Net.Http.Headers;

namespace ByteMarket.WebUI.Utilities.Handlers
{
	public class AuthTokenHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var jwt = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

			if (!string.IsNullOrEmpty(jwt))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
