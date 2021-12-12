using BikeScanner.Application.Interfaces;
using BikeScanner.DI;
using BikeScanner.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BikeScanner.Telegram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddBikeScanner(configuration);
                    services.AddServices();
                    services.AddVKServices(configuration);
                    services.AddPostgreDataAccess(configuration);

                    services.AddHostedService<TelegramHostedService>();


                    var provider = services.BuildServiceProvider();

                    //var repo = provider.GetRequiredService<ISubscriptionsRepository>();
                    //int counter = 1;
                    //while (counter++ < 1000)
                    //{
                    //    await repo.Add(new Domain.Models.SubscriptionEntity()
                    //    {
                    //        UserId = counter,
                    //        SearchQuery = "Canyon"
                    //    });
                    //}

                    //var indexer = provider.GetRequiredService<IContentIndexator>();
                    //indexer.Execute();

                    //var indexer = provider.GetRequiredService<INotificationsScheduler>();
                    //indexer.Execute();
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
