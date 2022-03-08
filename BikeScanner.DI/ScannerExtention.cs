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

            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IContentIndexatorJob, ContentIndexatorJob>();
            services.AddScoped<IAutoSearchJob, AutoSearchJob>();
            services.AddScoped<INotificationsSenderJob, NotificationsSenderJob>();
            services.AddScoped<ScanJobsChain>();
        }
    }
}
