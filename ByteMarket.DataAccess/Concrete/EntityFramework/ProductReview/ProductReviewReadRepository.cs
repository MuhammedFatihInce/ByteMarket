
using ByteMarket.DataAccess.Abstract.ProductReview;
using ByteMarket.DataAccess.Contexts;

namespace ByteMarket.DataAccess.Concrete.EntityFramework.ProductReview
{
	public class ProductReviewReadRepository : ReadRepository<Entities.Concrete.ProductReview>, IProductReviewReadRepository
	{
		public ProductReviewReadRepository(ByteMarketDbContext context): base(context)
		{
		}
	}
}
