
namespace ByteMarket.Entities.Concrete
{
	public class ProductImageFile : File
	{
		public bool Showcase { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
