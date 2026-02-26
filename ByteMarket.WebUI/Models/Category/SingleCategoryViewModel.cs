

namespace ByteMarket.WebUI.Models.Category
{
	public class SingleCategoryViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? Icon { get; set; }
		public CategoryImage.CategoryImageViewModel? CategoryImageFile { get; set; }
	}
}
