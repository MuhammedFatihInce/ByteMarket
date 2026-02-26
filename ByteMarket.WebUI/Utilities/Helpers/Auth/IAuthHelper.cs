using ByteMarket.WebUI.Models.User;

namespace ByteMarket.WebUI.Utilities.Helpers.Auth
{
	public interface IAuthHelper
	{
		UserHeaderViewModel GetUserFromCookie();
	}
}
