using ByteMarket.Entities.Common;
using ByteMarket.Entities.Concrete.Identity;

namespace ByteMarket.Entities.Concrete
{
	public class Basket : BaseEntity
	{
		public Guid UserId { get; set; }
		public AppUser User { get; set; }
		public Order Order { get; set; }
		public ICollection<BasketItem> BasketItems { get; set; }
	}
}
