
using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.Concrete;
using ByteMarket.Business.Concrete.Storage;
using ByteMarket.Business.Concrete.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ByteMarket.Business
{
	public static class ServiceRegistration
	{
		public static void AddBusinessServices(this IServiceCollection services)
		{
			services.AddScoped<IProductService, ProductManager>();
			services.AddScoped<IProductImageService, ProductImageManager>();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddScoped<IStorageService, StorageService>();
			services.AddScoped<IStorage, LocalStorage>();
		}
	}
}
