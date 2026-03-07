using ByteMarket.DataAccess.Abstract.Coupon;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Coupon
{
	public class CouponWriteRepository : WriteRepository<Entities.Concrete.Coupon>, ICouponWriteRepository
	{
		public CouponWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
