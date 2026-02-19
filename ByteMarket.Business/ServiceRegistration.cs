
using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ByteMarket.Business
{
	public static class ServiceRegistration
	{
		public static void AddBusinessServices(this IServiceCollection services)
		{
			services.AddScoped<IProductService, ProductManager>();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());
		}
	}
}
