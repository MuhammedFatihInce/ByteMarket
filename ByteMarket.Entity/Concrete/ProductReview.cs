
using ByteMarket.Entities.Common;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Entities.Concrete
{
	public class ProductReview:BaseEntity
	{
		public string Comment { get; set; }
		public int Rating { get; set; }
		public Guid ProductId { get; set; }
		public Product Product { get; set; }
		public Guid UserId { get; set; } 
		public AppUser User { get; set; }
	}
}
