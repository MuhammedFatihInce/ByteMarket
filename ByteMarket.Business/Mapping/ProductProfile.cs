using AutoMapper;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<CreateProductDto, Product>().ReverseMap();

			CreateMap<Product, ListProductDto>()
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
						src.ProductImageFiles != null && src.ProductImageFiles.Any()
							? $"{src.ProductImageFiles.FirstOrDefault().Path}"
							: null 
				));

			CreateMap<Product, SingleProductDto>().ReverseMap();
		}
	}
}
