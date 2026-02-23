using AutoMapper;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<ProductImageFile, DTOs.ProductImageDto.ProductImageDto>().ReverseMap();

			CreateMap<CreateProductDto, Product>()
				.ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ReverseMap(); 

			CreateMap<Product, ListProductDto>()
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
						src.ProductImageFiles != null && src.ProductImageFiles.Any()
							? $"{src.ProductImageFiles.FirstOrDefault().Path}"
							: null 
				));

			CreateMap<Product, SingleProductDto>().ReverseMap();

			CreateMap<UpdateProductDto, Product>().ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ReverseMap();
		}
	}
}
