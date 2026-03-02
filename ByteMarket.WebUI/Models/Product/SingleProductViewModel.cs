using ByteMarket.WebUI.Models.Category;
using ByteMarket.WebUI.Models.ProductImage;


namespace ByteMarket.WebUI.Models.Product
{
	public class SingleProductViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public List<ProductImageViewModel> ProductImageFiles { get; set; }
		public List<SingleCategoryViewModel> Categories { get; set; }
		public bool IsInWishlist { get; set; }

	}
}
