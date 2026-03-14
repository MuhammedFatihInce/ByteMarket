using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.User;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class UserManager : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public UserManager(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IResult> CreateAsync(CreateUserDto createUserDto)
		{
			AppUser user = new()
			{
				Id = Guid.NewGuid(), 
				UserName = createUserDto.Username,
				Email = createUserDto.Email,
				NameSurname = createUserDto.NameSurname,
			};

			IdentityResult result = await _userManager.CreateAsync(user, createUserDto.Password);

			if (result.Succeeded)
			{
				var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

				if (!roleResult.Succeeded)
					return new ErrorResult("Kullanıcı oluşturuldu ancak rol atanamadı.");

				return new SuccessResult("Kullanıcı başarıyla oluşturuldu.");
			}

			return new ErrorResult(string.Join(", ", result.Errors.Select(e => e.Description)));
		}

		public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime refreshTokenDate)
		{
			if (user != null)
			{
				user.RefreshToken = refreshToken;
				user.RefreshTokenEndDate = refreshTokenDate;
				await _userManager.UpdateAsync(user);
			}
		}

		public async Task<IDataResult<List<UserListDto>>> GetAllUsersWithRolesAsync()
		{
			var users = await _userManager.Users.ToListAsync();
			var userListDtos = new List<UserListDto>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userListDtos.Add(new UserListDto
				{
					Id = user.Id.ToString(),
					NameSurname = user.NameSurname,
					UserName = user.UserName,
					Email = user.Email,
					Roles = roles
				});
			}

			return new SuccessDataResult<List<UserListDto>>(userListDtos);
		}


		public async Task<IDataResult<List<GetAllUsersByFilterDto>>> GetAllUsersByFilterAsync(string q)
		{

			var users = await _userManager.Users.
				Where(x => string.IsNullOrEmpty(q) || x.NameSurname.Contains(q))
				.ToListAsync();

			var allRoles = await _roleManager.Roles.ToListAsync();

			var userListDtos = new List<GetAllUsersByFilterDto>();

			foreach (var user in users)
			{
				var roleNames = await _userManager.GetRolesAsync(user);

				var roleIds = allRoles
					.Where(r => roleNames.Contains(r.Name))
					.Select(r => r.Id.ToString())
					.ToList();

				userListDtos.Add(new GetAllUsersByFilterDto
				{
					Id = user.Id.ToString(),
					NameSurname = user.NameSurname,
					UserName = user.UserName,
					Email = user.Email,
					RoleIds = roleIds,
					RoleNames = roleNames.ToList()
				});
			}

			return new SuccessDataResult<List<GetAllUsersByFilterDto>>(userListDtos, "Kullanıcılar başarıyla listelendi.");
		}


		public async Task<IDataResult<List<UserListDto>>> GetAllUsersByRoleAsync(string roleName)
		{
			var users = await _userManager.GetUsersInRoleAsync(roleName);

			if (users == null)
				return new ErrorDataResult<List<UserListDto>>();

			var userListDtos = new List<UserListDto>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userListDtos.Add(new UserListDto
				{
					Id = user.Id.ToString(),
					NameSurname = user.NameSurname,
					UserName = user.UserName,
					Email = user.Email,
					Roles = roles
				});
			}

			return new SuccessDataResult<List<UserListDto>>(userListDtos);
		}
	}
}
