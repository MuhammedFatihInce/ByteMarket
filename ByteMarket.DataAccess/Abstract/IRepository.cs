using ByteMarket.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.DataAccess.Abstract
{
	public interface IRepository<T> where T : BaseEntity
	{
		DbSet<T> Table { get; }
	}
}
