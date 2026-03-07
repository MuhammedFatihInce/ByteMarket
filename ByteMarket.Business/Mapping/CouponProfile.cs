
using AutoMapper;
using ByteMarket.Business.DTOs.Coupon;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class CouponProfile:Profile
	{
		public CouponProfile()
		{
			CreateMap<CreateCouponDto, Coupon>()
				.ForMember(dest => dest.Products, opt => opt.Ignore())
				.ReverseMap();
		}
	}
}
