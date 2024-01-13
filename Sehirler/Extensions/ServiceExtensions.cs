using Microsoft.EntityFrameworkCore;
using Sehirler.Data;
using System.Text.Json.Serialization;

namespace Sehirler.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<DataContext>
			(options => 
			options.UseSqlServer(configuration.GetConnectionString("DefaultConection")));

		public static void ConfigureDataRepository(this IServiceCollection services) => services.AddScoped<IAppRepository, AppRepository>();
		public static void ConfigureIAutoRepository(this IServiceCollection services) => services.AddScoped<IAuthRepository, AuthRepository>();
		public static void ConfigureServicesJsonHanfd(IServiceCollection services)=>
		
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
				});
		
	}
}
