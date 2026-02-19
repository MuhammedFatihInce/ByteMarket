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

			CreateMap<Product, ListProductDto>().ReverseMap();

			CreateMap<Product, SingleProductDto>().ReverseMap();
		}
	}
}
