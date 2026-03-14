
namespace ByteMarket.Entities.Concrete
{
	public class ProductImageFile : File
	{
		public int DisplayOrder { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
