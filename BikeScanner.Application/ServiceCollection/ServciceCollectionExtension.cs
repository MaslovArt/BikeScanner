using BikeScanner.Application.Jobs;
using BikeScanner.Application.Services.SearchService;
using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Application.Services.UsersService;
using BikeScanner.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.Application.ServiceCollection
{
	public static class ServciceCollectionExtension
	{
		public static IServiceCollection AddBikeScannerMapping(
			this IServiceCollection services
			)
        {
			services.AddAutoMapper(typeof(MappingProfile));

			return services;
        }

		public static IServiceCollection AddBikeScannerServices(
			this IServiceCollection services
			)
        {
			services.AddTransient<ISubscriptionsService, SubscriptionsService>();
			services.AddTransient<ISearchService, SearchService>();
			services.AddTransient<IUsersService, UsersService>();

			return services;
        }

		public static IServiceCollection AddBikeScannerJobs(
			this IServiceCollection services
			)
        {
			services.AddTransient<IContentIndexatorJob, ContentIndexatorJob>();
			services.AddTransient<IAutoSearchJob, AutoSearchJob>();
			services.AddTransient<INotificationsSenderJob, NotificationsSenderJob>();

			return services;
        }
	}
}

