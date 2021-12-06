using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class ScannerExtention
    {
        public static void AddBikeScanner(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IContentIndexator, ContentIndexator>();
        }
    }
}
