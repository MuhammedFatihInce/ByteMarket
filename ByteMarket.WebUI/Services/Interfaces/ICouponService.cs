using ByteMarket.WebUI.Areas.Admin.Models.Coupon;
using ByteMarket.WebUI.Models.ResultModels;

namespace ByteMarket.WebUI.Services.Interfaces
{
	public interface ICouponService
	{
		Task<ApiDataResponse<string>> AddCategoryAsync(CreateCouponViewModel model);
		Task<ApiDataResponse<ListCouponViewModel>> GetCouponsForAdminAsync();
		Task<ApiDataResponse<SingleCouponViewModel>> GetCouponsForUpdateAsync(string id);
		Task<ApiDataResponse<object>> UpdateCouponAsync(UpdateCouponViewModel model);
		Task<ApiDataResponse<object>> DeleteCouponAsync(string id);
	}
}
