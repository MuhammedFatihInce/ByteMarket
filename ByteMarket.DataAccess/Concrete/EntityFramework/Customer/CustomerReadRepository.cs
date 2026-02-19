
using ByteMarket.DataAccess.Abstract.Customer;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Customer
{
	public class CustomerReadRepository: ReadRepository<Entities.Concrete.Customer>, ICustomerReadRepository
	{
		public CustomerReadRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
