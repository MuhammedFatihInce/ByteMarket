using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;

namespace ByteMarket.Business.Concrete
{
	public class UserManager : IUserService
	{
		private readonly UserManager<AppUser> _userManager;

		public UserManager(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IResult> CreateAsync(CreateUserDto createUserDto)
		{
			IdentityResult result = await _userManager.CreateAsync(new()
			{
				Id = Guid.NewGuid(),
				UserName = createUserDto.Username,
				Email = createUserDto.Email,
				NameSurname = createUserDto.NameSurname,
			}, createUserDto.Password);

			if (result.Succeeded)
				return new SuccessResult("Kullanıcı başarıyla oluşturuldu.");

			return new ErrorResult(string.Join(", ", result.Errors.Select(e => e.Description)));
		}

		public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnRefreshTokenDate)
		{
			if (user != null)
			{
				user.RefreshToken = refreshToken;
				user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnRefreshTokenDate);
				await _userManager.UpdateAsync(user);
			}
		}
	}
}
