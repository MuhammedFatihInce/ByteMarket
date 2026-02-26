
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Entities.Concrete.Identity;
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

		public TokenHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Token CreateAccessToken(int second, AppUser appUser)
		{
			Token token = new();

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

			token.Expiration = DateTime.UtcNow.AddSeconds(second);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
				new Claim(ClaimTypes.Email, appUser.Email),
				new Claim(ClaimTypes.Name, appUser.NameSurname)
			};

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
