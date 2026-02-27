
namespace ByteMarket.WebUI.Utilities.Helpers.Auth
{
	public interface IAuthHelper
	{
		Task SignInUserAsync(string usernameOrEmail, DateTime refreshTokenExpires, bool isPersistent = false);
		Task SignOutUserAsync();
	}
}
