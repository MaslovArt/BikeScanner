using BikeScanner.Application.Configs;
using BikeScanner.Application.Jobs;
using BikeScanner.Application.Services.SearchService;
using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Application.Services.UsersService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class ScannerExtention
    {
        public static void AddBikeScanner(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JobsConfig>(configuration.GetSection(nameof(JobsConfig)));

            services.AddTransient<ISubscriptionsService, SubscriptionsService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IUsersService, UsersService>();

            services.AddTransient<IContentIndexatorJob, ContentIndexatorJob>();
            services.AddTransient<IAutoSearchJob, AutoSearchJob>();
            services.AddTransient<INotificationsSenderJob, NotificationsSenderJob>();
            services.AddTransient<ScanJobsChain>();
        }
    }
}
