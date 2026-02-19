
using ByteMarket.DataAccess.Abstract.Customer;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Customer
{
	public class CustomerWriteRepository: WriteRepository<Entities.Concrete.Customer>, ICustomerWriteRepository
	{
		public CustomerWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
