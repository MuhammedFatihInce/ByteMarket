
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Business.Abstract
{
	public interface ITokenHandler
	{
		Token CreateAccessToken(int second, AppUser appUser);
		string CreateRefreshToken();
	}
}
