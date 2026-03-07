using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Abstract.BasketItem;
using ByteMarket.DataAccess.Abstract.Category;
using ByteMarket.DataAccess.Abstract.CategoryImageFile;
using ByteMarket.DataAccess.Abstract.Coupon;
using ByteMarket.DataAccess.Abstract.Customer;
using ByteMarket.DataAccess.Abstract.File;
using ByteMarket.DataAccess.Abstract.Order;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.DataAccess.Abstract.ProductImageFile;
using ByteMarket.DataAccess.Abstract.WishList;
using ByteMarket.DataAccess.Concrete.EntityFramework.Basket;
using ByteMarket.DataAccess.Concrete.EntityFramework.BasketItem;
using ByteMarket.DataAccess.Concrete.EntityFramework.Category;
using ByteMarket.DataAccess.Concrete.EntityFramework.CategoryImageFile;
using ByteMarket.DataAccess.Concrete.EntityFramework.Coupon;
using ByteMarket.DataAccess.Concrete.EntityFramework.Customer;
using ByteMarket.DataAccess.Concrete.EntityFramework.File;
using ByteMarket.DataAccess.Concrete.EntityFramework.Order;
using ByteMarket.DataAccess.Concrete.EntityFramework.Product;
using ByteMarket.DataAccess.Concrete.EntityFramework.ProductImageFile;
using ByteMarket.DataAccess.Concrete.EntityFramework.WishList;
using ByteMarket.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteMarket.DataAccess
{
	public static class ServiceRegistration
	{
		public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
		{
			// DbContext
			services.AddDbContext<ByteMarketDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));

			// Ürünler
			services.AddScoped<IProductReadRepository, ProductReadRepository>();
			services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

			// Siparişler
			services.AddScoped<IOrderReadRepository, OrderReadRepository>();
			services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

			// Müşteriler
			services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
			services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

			// Sepet ve Sepet Elemanları
			services.AddScoped<IBasketReadRepository, BasketReadRepository>();
			services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
			services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
			services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

			// Dosyalar
			services.AddScoped<IFileReadRepository, FileReadRepository>();
			services.AddScoped<IFileWriteRepository, FileWriteRepository>();
			services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
			services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
			services.AddScoped<ICategoryImageFileReadRepository, CategoryImageFileReadRepository>();
			services.AddScoped<ICategoryImageFileWriteRepository, CategoryImageFileWriteRepository>();

			// Kategoriler
			services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
			services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

			// İstek Listesi
			services.AddScoped<IWishListReadRepository, WishListReadRepository>();
			services.AddScoped<IWishListWriteRepository, WishListWriteRepository>();

			// Kupon
			services.AddScoped<ICouponReadRepository, CouponReadRepository>();
			services.AddScoped<ICouponWriteRepository, CouponWriteRepository>();


		}
	}
}
