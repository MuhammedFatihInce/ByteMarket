

using ByteMarket.DataAccess.Abstract.Category;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Category
{
	public class CategoryWriteRepository:WriteRepository<Entities.Concrete.Category>, ICategoryWriteRepository
	{
		public CategoryWriteRepository(ByteMarketDbContext context):base(context)
		{
		}
	}
}
