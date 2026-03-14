
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Business.Abstract
{
	public interface IUserService
	{
		Task<IResult> CreateAsync(CreateUserDto createUserDto);
		Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime refreshTokenDate);
		Task<IDataResult<List<UserListDto>>> GetAllUsersWithRolesAsync();
		Task<IDataResult<List<GetAllUsersByFilterDto>>> GetAllUsersByFilterAsync(string q);
		Task<IDataResult<List<UserListDto>>> GetAllUsersByRoleAsync(string roleName);
	}
}
