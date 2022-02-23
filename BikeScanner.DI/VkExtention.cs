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
        public static void AddVKAdSource(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VkApiAccessConfig>(configuration.GetSection(typeof(VkApiAccessConfig).Name));
            services.Configure<VkSourseConfig>(configuration.GetSection(typeof(VkSourseConfig).Name));

            services.AddSingleton<VKApi>();

            services.AddSingleton<IContentLoader, VkWallsContentLoadService>();
            //services.AddSingleton<IContentLoader, VkAlbumsContentLoadService>();
        }
    }
}
