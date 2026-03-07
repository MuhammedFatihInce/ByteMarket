
using ByteMarket.DataAccess.Abstract.Coupon;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Coupon
{
	public class CouponReadRepository : ReadRepository<Entities.Concrete.Coupon>, ICouponReadRepository
	{
		public CouponReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
