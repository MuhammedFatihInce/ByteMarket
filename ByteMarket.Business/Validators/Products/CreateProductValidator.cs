
using ByteMarket.Business.DTOs.Product;
using FluentValidation;

namespace ByteMarket.Business.Validators.Products
{
	public class CreateProductValidator : AbstractValidator<CreateProductDto>
	{
		public CreateProductValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty().WithMessage("Ürün adı boş geçilemez.")
				.MaximumLength(150).WithMessage("Ürün adı en fazla 150 karakter olabilir.");

			RuleFor(p => p.Price)
				.NotEmpty().WithMessage("Fiyat bilgisi gereklidir.")
				.GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

			RuleFor(p => p.Stock)
				.NotEmpty().WithMessage("Stok bilgisi gereklidir.")
				.GreaterThanOrEqualTo(0).WithMessage("Stok bilgisi negatif olamaz.");
		}
	}
}
