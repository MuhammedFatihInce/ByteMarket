namespace ByteMarket.WebUI.Areas.Admin.Models.Category
{
	public class UpdateCategoryViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? Icon { get; set; }
		public CategoryImage.CategoryImageAdminViewModel? CategoryImageFile { get; set; }
		public IFormFile? File { get; set; }

	}
}
