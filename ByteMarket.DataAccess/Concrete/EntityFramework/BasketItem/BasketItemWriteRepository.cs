using ByteMarket.DataAccess.Abstract.BasketItem;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.BasketItem
{
	public class BasketItemWriteRepository:WriteRepository<Entities.Concrete.BasketItem>, IBasketItemWriteRepository
	{
		public BasketItemWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
