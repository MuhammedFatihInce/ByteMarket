
using ByteMarket.DataAccess.Abstract.File;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.File
{
	public class FileWriteRepository: WriteRepository<Entities.Concrete.File>, IFileWriteRepository
	{
		public FileWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
