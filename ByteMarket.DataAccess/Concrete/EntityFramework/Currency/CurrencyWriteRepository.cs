
using ByteMarket.DataAccess.Abstract.Currency;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.Currency
{
	public class CurrencyWriteRepository : WriteRepository<Entities.Concrete.Currency>, ICurrencyWriteRepository
	{
	public CurrencyWriteRepository(ByteMarketDbContext context) : base(context)
	{
	}
	}
}
