using ByteMarket.DataAccess.Abstract.Currency;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Currency
{
	public class CurrencyReadRepository : ReadRepository<Entities.Concrete.Currency>, ICurrencyReadRepository
	{
		public CurrencyReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
