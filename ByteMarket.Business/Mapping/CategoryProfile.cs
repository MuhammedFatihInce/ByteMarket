
using AutoMapper;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class CategoryProfile:Profile
	{
		public CategoryProfile()
		{
			CreateMap<CreateCategoryDto, Category>().ReverseMap();

			CreateMap<Category, ListCategoryDto>()
				.ForMember(dest => dest.ProductCount,
					opt => opt.MapFrom(src => src.Products.Count));

			CreateMap<UpdateCategoryDto, Category>().ReverseMap();

			CreateMap<Category, SingleCategoryDto>().ReverseMap();
		}
	}
}
