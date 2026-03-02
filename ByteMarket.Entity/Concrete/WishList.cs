using ByteMarket.Entities.Common;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Entities.Concrete
{
	public class WishList:BaseEntity
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }

		public AppUser User { get; set; }
		public Product Product { get; set; }
	}
}
