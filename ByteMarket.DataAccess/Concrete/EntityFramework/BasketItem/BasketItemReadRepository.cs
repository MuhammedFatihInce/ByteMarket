
using ByteMarket.DataAccess.Abstract.BasketItem;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.BasketItem
{
	public class BasketItemReadRepository:ReadRepository<Entities.Concrete.BasketItem>, IBasketItemReadRepository
	{
		public BasketItemReadRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
