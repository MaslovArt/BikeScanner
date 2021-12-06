using BikeScanner.Application.Interfaces;
using BikeScanner.DI;
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

                    if (true)
                    {
                        var provider = services.BuildServiceProvider();
                        var indexer = provider.GetRequiredService<IContentIndexator>();
                        indexer.Execute(DateTime.Now.Date.AddDays(-30));
                    }
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
