
using ByteMarket.Business.DTOs.Category;

namespace ByteMarket.Business.DTOs.Product
{
	public class SingleProductDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public List<ProductImageDto.ProductImageDto> ProductImageFiles { get; set; }
		public List<SingleCategoryDto> Categories { get; set; }
		public bool IsInWishlist { get; set; }
		public bool IsPurchased { get; set; }

	}
}
