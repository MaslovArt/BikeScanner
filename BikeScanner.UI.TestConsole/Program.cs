using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Jobs;
using BikeScanner.DI;
using BikeScanner.Domain.Repositories;
using BikeScanner.Infrastructure.Notificators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BikeScanner.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices(async (hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddBikeScanner(configuration);
                    services.AddServices();
                    services.AddVKAdSource(configuration);
                    services.AddPostgreDataAccess(configuration);

                    var provider = services.BuildServiceProvider();

                    //var repo = provider.GetRequiredService<INotificationsQueueRepository>();
                    //int counter = 1;
                    //while (counter++ < 1000)
                    //{
                    //    await repo.Add(new Domain.Models.NotificationQueueEntity()
                    //    {
                    //        UserId = counter,
                    //        SearchQuery = "Canyon Spectral",
                    //        AdUrl = "vk.com....",
                    //        NotificationType = "test"
                    //    });
                    //}

                    //var repo = provider.GetRequiredService<ISubscriptionsRepository>();
                    //int counter = 1;
                    //while (counter++ < 1000)
                    //{
                    //    await repo.Add(new Domain.Models.SubscriptionEntity()
                    //    {
                    //        UserId = counter,
                    //        SearchQuery = "Canyon Spectral",
                    //        NotificationType = "test"
                    //    });
                    //}

                    //var service = provider.GetRequiredService<INotificationsSenderJob>();
                    //await service.Execute();

                    //var service = provider.GetRequiredService<IContentIndexatorJob>();
                    //await service.Execute();

                    //var service = provider.GetRequiredService<INotificationsSchedulerJob>();
                    //await service.Execute();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .Build()
                .RunAsync();
        }
    }
}
