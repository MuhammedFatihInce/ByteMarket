using ByteMarket.Business.DTOs.Coupon;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface ICouponService
	{
		Task<IResult> AddAsync(CreateCouponDto couponDto);
		Task<IDataResult<ListCouponDto>> GetAllAsync();
		Task<IDataResult<SingleCouponDto>> GetCouponByIdAsync(string id);
		Task<IResult> UpdateCouponAsync(UpdateCouponDto updateCouponDto);
		Task<IResult> DeleteAsync(string id);
	}
}
