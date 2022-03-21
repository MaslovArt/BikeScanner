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

            services.AddTransient<IContentsRepository, ContentsRepository>();
            services.AddTransient<INotificationsQueueRepository, NotificationsQueueRepository>();
            services.AddTransient<ISearchHistoryRepository, SearchHistoryRepository>();
            services.AddTransient<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddTransient<IVarsRepository, VarsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
        }
    }
}
