
using ByteMarket.DataAccess.Abstract.File;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.File
{
	public class FileReadRepository:ReadRepository<Entities.Concrete.File>, IFileReadRepository
	{
		public FileReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
