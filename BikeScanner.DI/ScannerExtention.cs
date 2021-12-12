using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Jobs;
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

            services.AddSingleton<IContentIndexatorJob, ContentIndexatorJob>();
            services.AddSingleton<INotificationsSchedulerJob, NotificationsSchedulerJob>();
        }
    }
}
