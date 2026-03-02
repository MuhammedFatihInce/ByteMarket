namespace ByteMarket.WebUI.Areas.Admin.Models.Category
{
	public class CreateCategoryViewModel
	{
		public string Name { get; set; }
		public string? Icon { get; set; }

		public IFormFile? File { get; set; }
	}
}
