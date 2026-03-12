using Microsoft.AspNetCore.Mvc.Rendering;

namespace ByteMarket.WebUI.Areas.Admin.Models.Product
{
	public class UpdateProductViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }

		public string? Description { get; set; }


		public List<string> CategoryIds { get; set; } = new();
		public List<SelectListItem>? CategoryList { get; set; }

		public List<ProductImage.ProductImageAdminViewModel>? ProductImageFiles { get; set; }

		public IFormFileCollection? Files { get; set; }

		public List<string>? DeletedImageIds { get; set; } = new List<string>();
	}
}
