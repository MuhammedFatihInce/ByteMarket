using ByteMarket.WebUI.Models.CategoryViewModels;
using ByteMarket.WebUI.Models.ProductImageViewModels;

namespace ByteMarket.WebUI.Models.ProductViewModels
{
	public class SingleProductViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public List<ProductImageViewModel> ProductImageFiles { get; set; }
		public List<SingleCategoryViewModel> Categories { get; set; }
	}
}
