
using ByteMarket.Entities.Common;

namespace ByteMarket.Entities.Concrete
{
	public class Category:BaseEntity
	{
		public string Name { get; set; }
		public string? Icon { get; set; }
		public ICollection<Product> Products { get; set; }
		public CategoryImageFile? CategoryImageFile { get; set; }
	}
}
