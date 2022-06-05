using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.Configs.DirtRu;
using BikeScanner.Infrastructure.Crawlers;
using BikeScanner.Infrastructure.ServiceCollection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestDirtRuCrawler();

            Console.ReadKey();
        }


        static ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                }))
                .AddLogging()
                .AddVkCrawlers(configuration)
                .BuildServiceProvider();

            return serviceProvider;
        }

        static async Task TestDirtRuCrawler()
        {
            var provider = BuildServiceProvider();

            var dirtRuCrawler = provider
                .GetRequiredService<IEnumerable<ICrawler>>()
                .First(s => s is DirtRuCrawler);
            var items = await dirtRuCrawler.Get(DateTime.Now.AddDays(-120));
            var uniq = items.Select(i => i.Url).Distinct().ToArray();
            var equals = items.Length == uniq.Length;
        }
    }
}