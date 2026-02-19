
using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Basket
{
	public class BasketReadRepository: ReadRepository<Entities.Concrete.Basket>, IBasketReadRepository
	{
		public BasketReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
