
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Role;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ByteMarket.Entities.Constants;

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

		public async Task<IResult> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
		{
			AppUser? user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
			if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

			IdentityResult result;

			if (assignRoleDto.IsAdding)
			{
				if (await _userManager.IsInRoleAsync(user, assignRoleDto.RoleName))
					return new SuccessResult("Kullanıcı zaten bu role sahip.");

				result = await _userManager.AddToRoleAsync(user, assignRoleDto.RoleName);
			}
			else
			{
				result = await _userManager.RemoveFromRoleAsync(user, assignRoleDto.RoleName);
			}

			
			if (result.Succeeded) return new SuccessResult("İşlem başarılı.");
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


		public async Task<IResult> UpdatePermissions(PermissionsUpdateDto dto)
		{

			var role = await _roleManager.FindByIdAsync(dto.Id);

			if (role == null)
			{
				return new ErrorResult("Yetki izni verilmek istenen rol bulunamadı.");
			}

			var existingClaims = await _roleManager.GetClaimsAsync(role);

			foreach (var claim in existingClaims)
			{
				await _roleManager.RemoveClaimAsync(role, claim);
			}

			foreach (var permission in dto.Permissions)
			{
				 await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
			}

			return new SuccessResult("Yetki izni verildi.");
		}

		public async Task<IDataResult<RolePermissionsDto>> GetPermissionsByRoleIdAsync(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null) return new ErrorDataResult<RolePermissionsDto>("Rol bulunamadı.");

			var existingClaims = await _roleManager.GetClaimsAsync(role);
			var existingPermissions = existingClaims
				.Where(c => c.Type == "Permission")
				.Select(c => c.Value)
				.ToList();

			var allPermissions = AuthorizePolicies.GetSystemPermissions();

			var model = new RolePermissionsDto
			{
				RoleId = role.Id.ToString(),
				RoleName = role.Name,
				Permissions = allPermissions.Select(p => new PermissionCheckDto
				{
					PermissionValue = p,
					IsExist = existingPermissions.Contains(p)
				}).ToList()
			};

			return new SuccessDataResult<RolePermissionsDto>(model);
		}

		
	}
}
