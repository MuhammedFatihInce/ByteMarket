
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Business.Abstract
{
	public interface IUserService
	{
		Task<IResult> CreateAsync(CreateUserDto createUserDto);
		Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnRefreshTokenDate);
	}
}
