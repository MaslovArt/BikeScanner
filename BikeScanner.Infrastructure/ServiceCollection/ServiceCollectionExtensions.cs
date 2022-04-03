using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.Notificators;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.Infrastructure.ServiceCollection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddNotificators(
			this IServiceCollection services
			)
        {
			services.AddSingleton<INotificator, TelegramNotificator>();

			return services;
        }
	}
}

