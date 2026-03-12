using ByteMarket.Entities.Common;

namespace ByteMarket.Entities.Concrete
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public ICollection<ProductImageFile> ProductImageFiles { get; set; }
		public ICollection<BasketItem> BasketItems { get; set; }
		public ICollection<Category> Categories { get; set; }
		public ICollection<WishList> WishList { get; set; }
		public ICollection<Coupon> Coupons { get; set; }
		public ICollection<ProductReview> Reviews { get; set; }
	}
}
