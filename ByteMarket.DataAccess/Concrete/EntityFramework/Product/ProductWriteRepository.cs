using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Product
{
	public class ProductWriteRepository: WriteRepository<Entities.Concrete.Product>, IProductWriteRepository
	{
		public ProductWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
