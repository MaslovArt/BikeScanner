using BikeScanner.Data.Postgre.Repositories;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.Data.Postgre.ServiceCollection
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddPostgresDB(
			this IServiceCollection services,
			IConfiguration configuration)
        {
			services.AddDbContext<BikeScannerContext>(options =>
			{
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddScoped<IActionsRepository, ActionsRepository>();
			services.AddScoped<IContentsRepository, ContentsRepository>();
			services.AddScoped<INotificationsQueueRepository, NotificationsQueueRepository>();
			services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
			services.AddScoped<IUsersRepository, UsersRepository>();
			services.AddScoped<IVarsRepository, VarsRepository>();

			return services;
        }
	}
}

