using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ByteMarket.WebUI.Utilities.Helpers.Auth
{
	public class AuthHelper : IAuthHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthHelper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task SignInUserAsync(string jwt, DateTime refreshTokenExpires, bool isPersistent = false)
		{
			var context = _httpContextAccessor.HttpContext;
			if (context == null || string.IsNullOrEmpty(jwt)) return;

			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadJwtToken(jwt);

			var claims = new List<Claim>();

			var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name")?.Value;
			if (!string.IsNullOrEmpty(name)) claims.Add(new Claim(ClaimTypes.Name, name));

			
			var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
			if (!string.IsNullOrEmpty(userId)) claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

			var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
			if (!string.IsNullOrEmpty(email)) claims.Add(new Claim(ClaimTypes.Email, email));

			var roles = token.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role");
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Value));
			}

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				IsPersistent = isPersistent,
				ExpiresUtc = refreshTokenExpires
			};

			
			await context.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);
		}

		public async Task SignOutUserAsync()
		{
			var context = _httpContextAccessor.HttpContext;
			if (context != null)
			{
				await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
		}
	}
}

