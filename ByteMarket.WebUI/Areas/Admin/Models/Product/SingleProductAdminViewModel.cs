using ByteMarket.WebUI.Areas.Admin.Models.Category;
using ByteMarket.WebUI.Areas.Admin.Models.ProductImage;


namespace ByteMarket.WebUI.Areas.Admin.Models.Product
{
	public class SingleProductAdminViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public List<ProductImageAdminViewModel> ProductImageFiles { get; set; }
		public List<SingleCategoryAdminViewModel> Categories { get; set; }
	}
}
