
using AutoMapper;
using ByteMarket.Business.DTOs.WishList;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Mapping
{
	public class WishListProfile:Profile
	{
		public WishListProfile()
		{
			CreateMap<WishList, WishListProduct>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId.ToString()))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
				.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Product.Stock))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
				.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
					src.Product.ProductImageFiles != null && src.Product.ProductImageFiles.Any()
						? src.Product.ProductImageFiles.OrderBy(x => x.DisplayOrder).Select(x => x.Path).Take(2).ToList()
						: null))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
					src.Product.Categories != null && src.Product.Categories.Any()
						? src.Product.Categories.FirstOrDefault().Name
						: "Kategori Yok"));
		}
	}
}