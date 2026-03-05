
using ByteMarket.Entities.Concrete.Identity;
using ByteMarket.Entities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace ByteMarket.DataAccess.SeedData
{
	public static class DbInitializer
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();

			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

			string[] roles = { "Admin", "Customer" };

			foreach (var roleName in roles)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new AppRole { Name = roleName });
				}
			}

			var adminEmail = "admin@mail.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				var newAdmin = new AppUser
				{
					UserName = "admin",
					Email = adminEmail,
					NameSurname = "Admin",
					EmailConfirmed = true
				};

				var result = await userManager.CreateAsync(newAdmin, "Admin1234");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(newAdmin, "Admin");
				}
			}

			var adminRole = await roleManager.FindByNameAsync("Admin");

			var allPermissions = AuthorizePolicies.GetSystemPermissions();

			foreach (var permission in allPermissions)
			{
				var claims = await roleManager.GetClaimsAsync(adminRole);
				if (!claims.Any(c => c.Type == "Permission" && c.Value == permission))
				{
					await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
				}
			}

		}
	}
}
