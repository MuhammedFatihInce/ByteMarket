
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Business.Abstract
{
	public interface ITokenHandler
	{
		Task<Token> CreateAccessToken(int second, int refreshTokenAddMinute, AppUser appUser);
		string CreateRefreshToken();
	}
}
