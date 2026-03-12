using AutoMapper;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			string currentUserId = null;

			CreateMap<ProductImageFile, DTOs.ProductImageDto.ProductImageDto>().ReverseMap();
		

			CreateMap<CreateProductDto, Product>()
				.ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ReverseMap(); 

			CreateMap<Product, ListProductDto>()
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
						src.ProductImageFiles != null && src.ProductImageFiles.Any()
							? src.ProductImageFiles.Select(x => x.Path).Take(2).ToList()
							: new List<string>()
				))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Categories.FirstOrDefault().Name))
				.ForMember(dest=> dest.IsInWishlist, opt=>opt.MapFrom(src=> currentUserId != null && src.WishList.Any(w=>w.UserId==Guid.Parse(currentUserId))))
				;

			CreateMap<Product, SingleProductDto>()
				.ForMember(dest => dest.ProductImageFiles, opt => opt.MapFrom(src => src.ProductImageFiles))
				.ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
				.ForMember(dest => dest.IsPurchased, opt => opt.Ignore())
				.ForMember(dest => dest.IsInWishlist, opt=> opt.MapFrom((src, dest, destMember, context) =>
				{
					var currentUserId = context.Items["CurrentUserId"] as string;
					if (string.IsNullOrEmpty(currentUserId)) return false;
					return src.WishList.Any(w => w.UserId == Guid.Parse(currentUserId));
				}));

			CreateMap<UpdateProductDto, Product>().ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ReverseMap();

			CreateMap<Product, GetAllProductByFilterDto>()
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
					src.ProductImageFiles != null && src.ProductImageFiles.Any()
						? $"{src.ProductImageFiles.FirstOrDefault().Path}"
						: null
				));
		}
	}
}
