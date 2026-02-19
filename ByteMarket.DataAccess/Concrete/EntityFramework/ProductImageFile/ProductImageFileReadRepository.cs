using ByteMarket.DataAccess.Abstract.ProductImageFile;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.ProductImageFile
{
	public class ProductImageFileReadRepository:ReadRepository<Entities.Concrete.ProductImageFile>, IProductImageFileReadRepository
	{
		public ProductImageFileReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
