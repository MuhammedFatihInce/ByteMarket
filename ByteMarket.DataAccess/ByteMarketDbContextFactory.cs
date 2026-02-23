using ByteMarket.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ByteMarket.DataAccess
{
	public class ByteMarketDbContextFactory : IDesignTimeDbContextFactory<ByteMarketDbContext>
	{
		public ByteMarketDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ByteMarket.WebAPI"))
				.AddJsonFile("appsettings.json")
				.Build();


			var optionsBuilder = new DbContextOptionsBuilder<ByteMarketDbContext>();
			var connectionString = configuration.GetConnectionString("SqlConnection");

			optionsBuilder.UseSqlServer(connectionString);

			return new ByteMarketDbContext(optionsBuilder.Options);
		}
	}
}
