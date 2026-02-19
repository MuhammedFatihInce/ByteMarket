

using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Basket
{
	public class BasketWriteRepository : WriteRepository<Entities.Concrete.Basket>, IBasketWriteRepository
	{
		public BasketWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
