using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Models.User;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface IUserService
	{
		Task<ApiDataResponse<List<UserListViewModel>>> GetAllUsersWithRolesAsync();
	}
}
