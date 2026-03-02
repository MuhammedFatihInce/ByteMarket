
using ByteMarket.DataAccess.Abstract.WishList;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.WishList
{
	public class WishListReadRepository: ReadRepository<Entities.Concrete.WishList>, IWishListReadRepository
	{
		public WishListReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
