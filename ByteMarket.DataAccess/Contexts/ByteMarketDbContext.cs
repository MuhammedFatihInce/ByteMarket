
using ByteMarket.Entities.Common;
using ByteMarket.Entities.Concrete;
using ByteMarket.Entities.Concrete.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.DataAccess.Contexts
{
	public class ByteMarketDbContext: IdentityDbContext<AppUser, AppRole, Guid>
	{
		public ByteMarketDbContext(DbContextOptions options):base(options)
		{
			
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Order>()
				.HasKey(b => b.Id);

			builder.Entity<Order>()
				.HasIndex(o => o.OrderCode)
				.IsUnique();

			builder.Entity<Basket>()
				.HasOne(b => b.Order)
				.WithOne(o => o.Basket)
				.HasForeignKey<Order>(b => b.Id);

			builder.Entity<Category>()
				.HasOne(c => c.CategoryImageFile) 
				.WithOne(ci => ci.Category)       
				.HasForeignKey<Category>("CategoryImageFileId");

			builder.Entity<Category>()
				.Property<Guid?>("CategoryImageFileId") 
				.IsRequired(false);

			builder.Entity<Category>()
				.HasIndex("CategoryImageFileId")
				.IsUnique();

			base.OnModelCreating(builder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var datas = ChangeTracker.Entries<BaseEntity>();

			foreach (var data in datas)
			{
				_ = data.State switch
				{
					EntityState.Added => data.Entity.CreateDate = DateTime.UtcNow,
					EntityState.Modified => data.Entity.UpdateDate = DateTime.UtcNow,
					_ => DateTime.UtcNow
				};
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
