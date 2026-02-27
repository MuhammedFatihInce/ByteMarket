
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Role;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class RoleManager : IRoleService
	{
		private readonly RoleManager<AppRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;

		public RoleManager(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<IResult> CreateRoleAsync(string name)
		{
			IdentityResult result = await _roleManager.CreateAsync(new AppRole { Name = name });
			if (result.Succeeded) return new SuccessResult("Rol başarıyla oluşturuldu.");
			return new ErrorResult("Rol oluşturulamadı.");
		}

		public async Task<IDataResult<List<RoleListDto>>> GetAllRolesAsync()
		{
			var roles = await _roleManager.Roles.Select(r => new RoleListDto
			{
				Id = r.Id.ToString(),
				Name = r.Name
			}).ToListAsync();
			return new SuccessDataResult<List<RoleListDto>>(roles);
		}

		public async Task<IResult> AssignRoleToUserAsync(string userId, string[] roles)
		{
			AppUser? user = await _userManager.FindByIdAsync(userId);
			if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

			var userRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, userRoles);

			IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
			if (result.Succeeded) return new SuccessResult("Roller başarıyla atandı.");
			return new ErrorResult("Rol atama işlemi başarısız.");
		}

		public async Task<IResult> DeleteRoleAsync(string id)
		{
			AppRole? role = await _roleManager.FindByIdAsync(id);

			if (role == null)
			{
				return new ErrorResult("Silinmek istenen rol bulunamadı.");
			}

			if (role.Name == "Admin" || role.Name == "Customer")
			{
				return new ErrorResult("Sistem için kritik olan temel roller silinemez.");
			}

			IdentityResult result = await _roleManager.DeleteAsync(role);

			if (result.Succeeded)
			{
				return new SuccessResult("Rol başarıyla sistemden silindi.");
			}

			return new ErrorResult("Rol silinirken bir hata oluştu.");
		}
	}
}
