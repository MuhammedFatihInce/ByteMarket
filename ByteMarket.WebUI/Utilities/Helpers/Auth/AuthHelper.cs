using ByteMarket.WebUI.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ByteMarket.WebUI.Utilities.Helpers.Auth
{
	public class AuthHelper : IAuthHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthHelper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public UserHeaderViewModel GetUserFromCookie()
		{
			var userViewModel = new UserHeaderViewModel { IsAuthenticated = false };
			var jwt = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];

			if (string.IsNullOrEmpty(jwt)) return userViewModel;

			try
			{
				var handler = new JwtSecurityTokenHandler();
				var token = handler.ReadJwtToken(jwt);

				var nameClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name")?.Value;
				

				if (nameClaim != null)
				{
					userViewModel.IsAuthenticated = true;
					userViewModel.NameSurname = nameClaim;
				}
			}
			catch
			{
				// ilerde loglanacak
			}

			return userViewModel;
		}
	}
}
