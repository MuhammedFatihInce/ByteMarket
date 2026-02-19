
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Product
{
	public class ProductReadRepository: ReadRepository<Entities.Concrete.Product>, IProductReadRepository
	{
		public ProductReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
