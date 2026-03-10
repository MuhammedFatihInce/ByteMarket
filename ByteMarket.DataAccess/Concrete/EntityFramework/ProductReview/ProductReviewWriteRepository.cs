

using ByteMarket.DataAccess.Abstract.ProductReview;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.ProductReview
{
	public class ProductReviewWriteRepository : WriteRepository<Entities.Concrete.ProductReview>, IProductReviewWriteRepository
	{
		public ProductReviewWriteRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
