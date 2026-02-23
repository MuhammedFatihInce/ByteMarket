
using ByteMarket.DataAccess.Abstract.Category;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Category
{
	public class CategoryReadRepository:ReadRepository<Entities.Concrete.Category>, ICategoryReadRepository
	{
		public CategoryReadRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
