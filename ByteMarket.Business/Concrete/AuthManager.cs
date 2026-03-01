
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Token;
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace ByteMarket.Business.Concrete
{
	public class AuthManager : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		private readonly IMailService _mailService;

		public AuthManager(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IUserService userService, IConfiguration configuration, IMailService mailService)
		{
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_userService = userService;
			_configuration = configuration;
			_mailService = mailService;
		}

		public async Task<IDataResult<Token>> LoginAsync(LoginUserDto loginUserDto)
		{
			AppUser user = await _userManager.FindByNameAsync(loginUserDto.UsernameOrEmail);
			if (user == null) user = await _userManager.FindByEmailAsync(loginUserDto.UsernameOrEmail);

			if (user == null) return new ErrorDataResult<Token>("Kullanıcı adı veya şifre hatalı.");

			bool checkPassword = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

			if (checkPassword)
			{
				Token token = await _tokenHandler.CreateAccessToken(60, 300, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.RefreshTokenExpiration);
				return new SuccessDataResult<Token>(token, "Giriş başarılı.");
			}

			return new ErrorDataResult<Token>("Kullanıcı adı veya şifre hatalı.");
		}

		public async Task<IDataResult<Token>> RefreshTokenLoginAsync(string refreshToken)
		{
			AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

			if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
			{
				Token token = await _tokenHandler.CreateAccessToken(60, 300, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.RefreshTokenExpiration);
				return new SuccessDataResult<Token>(token, "Token yenilendi.");
			}

			return new ErrorDataResult<Token>("Geçersiz veya süresi dolmuş refresh token!");
		}

		public async Task<IResult> LogoutAsync(string userId, string refreshToken)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

			if (user.RefreshToken != refreshToken)
			{
				return new ErrorResult("Geçersiz token veya oturum zaten sonlandırılmış.");
			}

			user.RefreshToken = null;
			user.RefreshTokenEndDate = null;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
				return new SuccessResult("Çıkış yapıldı ve refresh token geçersiz kılındı.");

			return new ErrorResult("Kullanıcı bulunamadı veya zaten çıkış yapılmış.");
		}

		public async Task<IDataResult<Token>> GoogleLoginAsync(string idToken)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { _configuration["ExternalLogin:Google:ClientId"] }
			};

			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

			var user = await _userManager.FindByLoginAsync("GOOGLE", payload.Subject);

			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(payload.Email);

				if (user == null)
				{
					user = new AppUser
					{
						Id = Guid.NewGuid(),
						Email = payload.Email,
						UserName = payload.Email,
						NameSurname = $"{payload.GivenName} {payload.FamilyName}"
					};

					var createResult = await _userManager.CreateAsync(user);

					if (!createResult.Succeeded) return new ErrorDataResult<Token>("Kullanıcı oluşturulamadı.");

					var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

					if (!roleResult.Succeeded)
						return new ErrorDataResult<Token>("Kullanıcı oluşturuldu ancak rol atanamadı.");
				}

				await _userManager.AddLoginAsync(user, new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE"));
			}

			var token = await _tokenHandler.CreateAccessToken(60, 300, user);
			await _userService.UpdateRefreshToken(token.RefreshToken, user, token.RefreshTokenExpiration);
			return new SuccessDataResult<Token>(token, "Google ile giriş başarılı.");
		}


		public async Task<IResult> PasswordResetAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

			string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
			string encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);

			string webUiUrl = _configuration["ClientSettings:WebUI_Url"];
			string resetLink = $"{webUiUrl}/Account/ResetPassword?email={email}&token={encodedToken}";

			await _mailService.SendPasswordResetMailAsync(email, resetLink);

			return new SuccessResult("Şifre yenileme bağlantısı e-posta adresinize gönderildi.");
		}

		public async Task<IResult> VerifyResetTokenAsync(ResetPasswordDto resetPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
			if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

			byte[] decodedTokenBytes = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
			string originalToken = Encoding.UTF8.GetString(decodedTokenBytes);

			var result = await _userManager.ResetPasswordAsync(user, originalToken, resetPasswordDto.NewPassword);

			if (result.Succeeded)
				return new SuccessResult("Şifreniz başarıyla güncellendi.");

			return new ErrorResult("Şifre güncellenirken bir hata oluştu veya bağlantı süresi dolmuş.");
		}

	}
}
