using ByteMarket.Business.DTOs.Token;
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface IAuthService
	{
		Task<IDataResult<Token>> LoginAsync(LoginUserDto loginUserDto);
		Task<IDataResult<Token>> RefreshTokenLoginAsync(string refreshToken);
		Task<IResult> LogoutAsync(string userId, string refreshToken);
		Task<IDataResult<Token>> GoogleLoginAsync(string idToken);
		Task<IResult> PasswordResetAsync(string email);
		Task<IResult> VerifyResetTokenAsync(ResetPasswordDto resetPasswordDto);
	}
}
