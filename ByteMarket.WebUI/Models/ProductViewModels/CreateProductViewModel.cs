using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ByteMarket.WebUI.Models.ProductViewModels
{
	public class CreateProductViewModel
	{
		[Required(ErrorMessage = "Ürün adı boş geçilemez.")]
		[Display(Name = "Ürün Adı")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Fiyat bilgisi zorunludur.")]
		[Range(0.1, 1000000, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
		[Display(Name = "Satış Fiyatı")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Stok adedi zorunludur.")]
		[Range(0, 100000, ErrorMessage = "Stok 0 ile 100.000 arasında olmalıdır.")]
		[Display(Name = "Stok Miktarı")]
		public int Stock { get; set; }

		public List<string> CategoryIds { get; set; }

		public List<SelectListItem>? CategoryList { get; set; }

		public IFormFileCollection? Files { get; set; }
	}
}
