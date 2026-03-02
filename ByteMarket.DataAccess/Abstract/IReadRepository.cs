using ByteMarket.Entities.Common;
using System.Linq.Expressions;

namespace ByteMarket.DataAccess.Abstract
{
	public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
	{
		IQueryable<T> GetAll(bool tracking = true);
		IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
		Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
		Task<T> GetByIdAsync(string id, bool tracking = true);
		Task<bool> AnyAsync(Expression<Func<T, bool>> method, bool tracking = true);
	}
}
