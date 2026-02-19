
using ByteMarket.DataAccess.Abstract;
using ByteMarket.DataAccess.Contexts;
using ByteMarket.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ByteMarket.DataAccess.Concrete.EntityFramework
{
	public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
	{
		private readonly ByteMarketDbContext _context;

		public WriteRepository(ByteMarketDbContext context)
		{
			_context = context;
		}

		public DbSet<T> Table => _context.Set<T>();

		public async Task<bool> AddAsync(T model)
		{
			EntityEntry<T> entityEntry = await Table.AddAsync(model);
			return entityEntry.State == EntityState.Added;
		}

		public async Task<bool> AddRangeAsync(List<T> datas)
		{
			await Table.AddRangeAsync(datas);
			return true;
		}

		public bool Remove(T model)
		{
			EntityEntry<T> entityEntry = Table.Remove(model);
			return entityEntry.State == EntityState.Deleted;
		}

		public async Task<bool> RemoveAsync(string id)
		{
			T model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
			return Remove(model);
		}

		public bool RemoveRange(List<T> datas)
		{
			Table.RemoveRange(datas);
			return true;
		}

		public bool Update(T model)
		{
			EntityEntry entityEntry = Table.Update(model);
			return entityEntry.State == EntityState.Modified;
		}

		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
