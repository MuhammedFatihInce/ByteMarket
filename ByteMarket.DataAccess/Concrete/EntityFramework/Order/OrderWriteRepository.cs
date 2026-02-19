
using ByteMarket.DataAccess.Abstract.Order;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Order
{
	public class OrderWriteRepository:WriteRepository<Entities.Concrete.Order>,IOrderWriteRepository
	{
		public OrderWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
