
namespace ByteMarket.Business.DTOs.Category
{
	public class SingleCategoryDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string? Icon { get; set; }
		public CategoryImageDto.CategoryImageDto? CategoryImageFile { get; set; }
	}
}
