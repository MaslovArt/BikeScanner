using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.BotService.CommandsHandler;
using BikeScanner.UI.Bot.BotService.Config;
using BikeScanner.UI.Bot.BotService.Context;
using BikeScanner.UI.Bot.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BikeScanner.DI
{
    public static class TelegramUIExtention
    {
        public static void AddTelegramUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TelegramConfig>(configuration.GetSection(nameof(TelegramConfig)));

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => {
                var bot = x.GetRequiredService<IOptions<TelegramConfig>>().Value;

                return new TelegramBotClient(bot.Key);
            });

            services.AddSingleton<IBotContext, MemoryBotContext>();
            services.AddScoped<CommandsHandler>();

            services.AddScoped<IStartBotCommand, StartCommand>();
            services.AddScoped<ICancelCommand, CancelCommand>();
            services.AddScoped<IHelpBotCommand, HelpCommand>();
            services.AddScoped<IUnknownCommand, UnknownCommand>();
        }
    }
}
