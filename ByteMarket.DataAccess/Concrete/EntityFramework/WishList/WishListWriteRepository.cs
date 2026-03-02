
using ByteMarket.DataAccess.Abstract.WishList;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.WishList
{
	public class WishListWriteRepository : WriteRepository<Entities.Concrete.WishList>, IWishListWriteRepository
	{
		public WishListWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
