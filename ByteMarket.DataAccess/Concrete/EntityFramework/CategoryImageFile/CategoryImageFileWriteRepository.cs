
using ByteMarket.DataAccess.Abstract.CategoryImageFile;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.CategoryImageFile
{
	public class CategoryImageFileWriteRepository: WriteRepository<Entities.Concrete.CategoryImageFile>, ICategoryImageFileWriteRepository
	{
		public CategoryImageFileWriteRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
