
using ByteMarket.DataAccess.Abstract.CategoryImageFile;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.CategoryImageFile
{
	public class CategoryImageFileReadRepository:ReadRepository<Entities.Concrete.CategoryImageFile>, ICategoryImageFileReadRepository
	{
		public CategoryImageFileReadRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
