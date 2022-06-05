using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.Configs.DirtRu;
using BikeScanner.Infrastructure.Configs.Vk;
using BikeScanner.Infrastructure.Crawlers;
using BikeScanner.Infrastructure.Notificators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.Infrastructure.ServiceCollection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTelegramNotificator(this IServiceCollection services)
        {
			services.AddSingleton<INotificator, TelegramNotificator>();

			return services;
        }

		public static IServiceCollection AddLogNotificator(this IServiceCollection services)
		{
			services.AddSingleton<INotificator, LogNotificator>();

			return services;
		}

		public static IServiceCollection AddVkCrawlers(
			this IServiceCollection services,
			IConfiguration configuration
			)
        {
			services.Configure<VkApiAccessConfig>(configuration.GetSection(nameof(VkApiAccessConfig)));
			services.Configure<VkSourseConfig>(configuration.GetSection(nameof(VkSourseConfig)));
			services.Configure<DirtRuSourceConfig>(configuration.GetSection(nameof(DirtRuSourceConfig)));

			services.AddTransient<ICrawler, VkPostsCrawler>();
			//services.AddTransient<ICrawler, VkPhotosCrawler>();
			services.AddTransient<ICrawler, DirtRuCrawler>();

			return services;
        }
	}
}

