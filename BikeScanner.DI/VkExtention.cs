using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.VK.Api;
using BikeScanner.Infrastructure.VK.Config;
using BikeScanner.Infrastructure.VK.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class VkExtention
    {
        public static void AddVKServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VkSettings>(configuration.GetSection(typeof(VkSettings).Name));
            services.Configure<VkSourseSettings>(configuration.GetSection(typeof(VkSourseSettings).Name));

            services.AddSingleton<VKApi>();

            services.AddSingleton<IContentLoader, VkWallsContentLoadService>();
            services.AddSingleton<IContentLoader, VkAlbumsContentLoadService>();
        }
    }
}
