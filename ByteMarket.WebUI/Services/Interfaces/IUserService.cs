using ByteMarket.WebUI.Areas.Admin.Models.User;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IUserService
	{
		Task<ApiDataResponse<List<UserListViewModel>>> GetAllUsersWithRolesAsync();
	}
}
