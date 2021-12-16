using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using BikeScanner.Infrastructure.Loggers;
using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.Notificators;

namespace BikeScanner.DI
{
    public static class ServicesExtention
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimeLogger<>)));

            services.AddSingleton<INotificator, TestLoggerNotificator>();
        }
    }
}
