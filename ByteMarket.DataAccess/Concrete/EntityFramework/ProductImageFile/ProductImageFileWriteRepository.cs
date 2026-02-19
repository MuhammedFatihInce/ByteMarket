
using ByteMarket.DataAccess.Abstract.ProductImageFile;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.ProductImageFile
{
	public class ProductImageFileWriteRepository: WriteRepository<Entities.Concrete.ProductImageFile>, IProductImageFileWriteRepository
	{
		public ProductImageFileWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
