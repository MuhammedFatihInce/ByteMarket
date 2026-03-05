
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ByteMarket.Business.Concrete
{
	public class TokenHandler: ITokenHandler
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public TokenHandler(IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_configuration = configuration;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<Token> CreateAccessToken(int minutes, int refreshTokenAddDays, AppUser appUser)
		{
			Token token = new();

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

			token.Expiration = DateTime.UtcNow.AddMinutes(minutes);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
				new Claim(ClaimTypes.Email, appUser.Email),
				new Claim(ClaimTypes.Name, appUser.NameSurname)
			};

			var userRoles = await _userManager.GetRolesAsync(appUser);
			foreach (var roleName in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, roleName));

				var role = await _roleManager.FindByNameAsync(roleName);
				if (role != null)
				{
					var roleClaims = await _roleManager.GetClaimsAsync(role);
					foreach (var roleClaim in roleClaims)
					{
						if (!claims.Any(c => c.Type == roleClaim.Type && c.Value == roleClaim.Value))
						{
							claims.Add(roleClaim);
						}
					}
				}

			}

			JwtSecurityToken jwt = new(
				issuer: _configuration["Token:Issuer"],
				audience: _configuration["Token:Audience"],
				expires: token.Expiration,
				notBefore: DateTime.UtcNow,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
				claims: claims
			);

			JwtSecurityTokenHandler tokenHandler = new();
			token.AccessToken = tokenHandler.WriteToken(jwt);

			token.RefreshTokenExpiration = token.Expiration.AddDays(refreshTokenAddDays);
			token.RefreshToken = CreateRefreshToken();

			return token;
		}

		public string CreateRefreshToken()
		{
			byte[] number = new byte[32];
			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(number);
			return Convert.ToBase64String(number);
		}
	}
}
