using BikeScanner.Application.Jobs;
using BikeScanner.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.TelegramPoll
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
                    services.AddVKAdSource(configuration);
                    services.AddPostgreDataAccess(configuration);
                    services.AddTelegramUI(configuration);
                    services.AddServices();

                    //try
                    //{
                    //    var provider = services.BuildServiceProvider();
                    //    var indexator = provider.GetRequiredService<IContentIndexatorJob>();
                    //    await indexator.Execute();
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    services.AddHostedService<TelegramHostedService>();
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
