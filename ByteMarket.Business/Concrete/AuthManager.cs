
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class AuthManager : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IUserService _userService; 

		public AuthManager(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IUserService userService)
		{
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_userService = userService;
		}

		public async Task<IDataResult<Token>> LoginAsync(LoginUserDto loginUserDto)
		{
			AppUser user = await _userManager.FindByNameAsync(loginUserDto.UsernameOrEmail);
			if (user == null) user = await _userManager.FindByEmailAsync(loginUserDto.UsernameOrEmail);

			if (user == null) return new ErrorDataResult<Token>("Kullanıcı adı veya şifre hatalı.");

			bool checkPassword = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

			if (checkPassword)
			{
				Token token = _tokenHandler.CreateAccessToken(900, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
				return new SuccessDataResult<Token>(token, "Giriş başarılı.");
			}

			return new ErrorDataResult<Token>("Kullanıcı adı veya şifre hatalı.");
		}

		public async Task<IDataResult<Token>> RefreshTokenLoginAsync(string refreshToken)
		{
			AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

			if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
			{
				Token token = _tokenHandler.CreateAccessToken(900, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
				return new SuccessDataResult<Token>(token, "Token yenilendi.");
			}

			return new ErrorDataResult<Token>("Geçersiz veya süresi dolmuş refresh token!");
		}

		public async Task<IResult> LogoutAsync(string refreshToken)
		{
			AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

			if (user != null)
			{
				user.RefreshToken = null;
				user.RefreshTokenEndDate = null;

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
					return new SuccessResult("Çıkış yapıldı ve refresh token geçersiz kılındı.");
			}

			return new ErrorResult("Kullanıcı bulunamadı veya zaten çıkış yapılmış.");
		}
	}
}
