
using AutoMapper;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Business.DTOs.CategoryImageDto;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class CategoryProfile:Profile
	{
		public CategoryProfile()
		{
			CreateMap<CategoryImageFile, CategoryImageDto>().ReverseMap();

			CreateMap<CreateCategoryDto, Category>().ReverseMap();

			CreateMap<Category, ListCategoryDto>()
				.ForMember(dest => dest.ProductCount,
					opt => opt.MapFrom(src => src.Products.Count.ToString()))
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
						src.CategoryImageFile != null ? src.CategoryImageFile.Path : null
				));

			CreateMap<UpdateCategoryDto, Category>().ReverseMap();

			CreateMap<Category, SingleCategoryDto>()
				.ForMember(dest => dest.CategoryImageFile, opt => opt.MapFrom(src => src.CategoryImageFile));
		}
	}
}
	