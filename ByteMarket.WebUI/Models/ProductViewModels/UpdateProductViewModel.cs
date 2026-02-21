namespace ByteMarket.WebUI.Models.ProductViewModels
{
	public class UpdateProductViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }

		public List<ProductImageViewModels.ProductImageViewModel>? ProductImageFiles { get; set; }

		public IFormFileCollection? Files { get; set; }

		public List<string>? DeletedImageIds { get; set; } = new List<string>();
	}
}
