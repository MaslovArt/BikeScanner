using BikeScanner.Data.Postgre;
using BikeScanner.Data.Postgre.Repositories;
using BikeScanner.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class PostgreDataExtention
    {
        public static void AddPostgreDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BikeScannerContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IContentsRepository, ContentsRepository>();
            services.AddScoped<INotificationsQueueRepository, NotificationsQueueRepository>();
            services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddScoped<IVarsRepository, VarsRepository>();
        }
    }
}
