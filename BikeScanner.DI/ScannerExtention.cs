using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Jobs;
using BikeScanner.Application.Services;
using BikeScanner.Application.Services.NotificationFactory;
using BikeScanner.Application.Services.SearchService;
using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Domain.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class ScannerExtention
    {
        public static void AddBikeScanner(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BSSettings>(configuration.GetSection(nameof(BSSettings)));

            services.AddSingleton<INotificatorFactory, NotificatorFactory>();

            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddScoped<IContentIndexatorJob, ContentIndexatorJob>();
            services.AddScoped<INotificationsSchedulerJob, NotificationsSchedulerJob>();
            services.AddScoped<INotificationsSenderJob, NotificationsSenderJob>();
        }
    }
}
