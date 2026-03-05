
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Business.Abstract
{
	public interface ITokenHandler
	{
		Task<Token> CreateAccessToken(int minutes, int refreshTokenAddDays, AppUser appUser);
		string CreateRefreshToken();
	}
}
