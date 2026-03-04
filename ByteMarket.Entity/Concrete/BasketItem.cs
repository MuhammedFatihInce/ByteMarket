using ByteMarket.Entities.Common;

namespace ByteMarket.Entities.Concrete
{
	public class BasketItem : BaseEntity
	{
		public Guid BasketId { get; set; }
		public Guid ProductId { get; set; }

		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public Basket Basket { get; set; }
		public Product Product { get; set; }
	}
}
