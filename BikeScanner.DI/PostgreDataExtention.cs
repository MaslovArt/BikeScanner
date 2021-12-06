using BikeScanner.Data.Postgre;
using BikeScanner.Data.Postgre.Repositories;
using BikeScanner.Domain.Content;
using BikeScanner.Domain.NotificationsQueue;
using BikeScanner.Domain.SearchHistory;
using BikeScanner.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BikeScanner.DI
{
    public static class PostgreDataExtention
    {
        public static void AddPostgreDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DBSettings>(configuration.GetSection("ConnectionStrings"));

            services.AddDbContext<BikeScannerContext>((provided, options) =>
            {
                var db = provided.GetRequiredService<IOptions<DBSettings>>().Value;

                options.UseNpgsql(db.DefaultConnection);
            });

            services.AddScoped<IContentsRepository, ContentsRepository>();
            services.AddScoped<INotificationsQueueRepository, NotificationsQueueRepository>();
            services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
        }
    }
}
