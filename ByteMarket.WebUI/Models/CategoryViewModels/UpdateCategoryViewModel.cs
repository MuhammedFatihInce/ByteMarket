namespace ByteMarket.WebUI.Models.CategoryViewModels
{
	public class UpdateCategoryViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? Icon { get; set; }
		public CategoryImageViewModel.CategoryImageViewModel? CategoryImageFile { get; set; }
		public IFormFile? File { get; set; }

	}
}
