
namespace ByteMarket.Entities.Constants
{
	public static class AuthorizePolicies
	{
		public const string AdminOnly = "AdminOnly";
		public const string CustomerOnly = "CustomerOnly";

		public const string FullProductManagement = "FullProductManagement";
		public const string FullCategoryManagement = "FullCategoryManagement";
		public const string FullUserManagement = "FullUserManagement";
		public const string FullRoleManagement = "FullRoleManagement";

		public const string ProductsPermissions = "Permissions.Products";
		public const string CategoriesPermissions = "Permissions.Categories";
		public const string UsersPermissions = "Permissions.Users";
		public const string RolesPermissions = "Permissions.Roles";

		public static List<string> GetSystemPermissions()
		{
			return new List<string>
			{
				CategoriesPermissions,
				ProductsPermissions,
				UsersPermissions,
				RolesPermissions
			};
		}
	}
}
