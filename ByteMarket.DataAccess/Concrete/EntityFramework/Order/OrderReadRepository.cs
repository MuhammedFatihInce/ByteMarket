
using ByteMarket.DataAccess.Abstract.Order;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Order
{
	public class OrderReadRepository: ReadRepository<Entities.Concrete.Order>, IOrderReadRepository
	{
		public OrderReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
