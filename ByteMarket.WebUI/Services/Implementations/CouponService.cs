
using ByteMarket.WebUI.Areas.Admin.Models.Coupon;
using ByteMarket.WebUI.Areas.Admin.Models.Product;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class CouponService:ICouponService
	{
		private readonly IApiService _apiService;
		public CouponService(IApiService apiService) => _apiService = apiService;

		public async Task<ApiDataResponse<string>> AddCategoryAsync(CreateCouponViewModel model)
		{
			return await _apiService.PostAsync<string>("Coupon", model);
		}

		public async Task<ApiDataResponse<ListCouponViewModel>> GetCouponsForAdminAsync()
		{
			return await _apiService.GetAsync<ListCouponViewModel>("Coupon");
		}

		public async Task<ApiDataResponse<SingleCouponViewModel>> GetCouponsForUpdateAsync(string id)
		{
			return await _apiService.GetByIdAsync<SingleCouponViewModel>("Coupon", id);
		}

		public async Task<ApiDataResponse<object>> UpdateCouponAsync(UpdateCouponViewModel model)
		{
			return await _apiService.PutAsync<object>("Coupon", model);
		}

		public async Task<ApiDataResponse<object>> DeleteCouponAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("Coupon", id);
		}
	}
}
