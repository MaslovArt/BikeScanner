using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.BotService.CommandsHandler;
using BikeScanner.UI.Bot.BotService.Config;
using BikeScanner.UI.Bot.BotService.Context;
using BikeScanner.UI.Bot.Commands;
using BikeScanner.UI.Bot.Commands.Search;
using BikeScanner.UI.Bot.Configs;
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
            services.Configure<TelegramUIConfig>(configuration.GetSection(nameof(TelegramUIConfig)));
            services.Configure<TelegramBotConfig>(configuration.GetSection(nameof(TelegramBotConfig)));

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => {
                var bot = x.GetRequiredService<IOptions<TelegramBotConfig>>().Value;

                return new TelegramBotClient(bot.Key);
            });

            services.AddSingleton<IBotContext, MemoryBotContext>();
            services.AddScoped<CommandsHandler>();

            #region base commands
            services.AddScoped<IStartBotCommand, StartCommand>();
            services.AddScoped<ICancelCommand, CancelCommand>();
            services.AddScoped<IHelpBotCommand, HelpCommand>();
            services.AddScoped<IUnknownCommand, UnknownCommand>();
            #endregion
            #region search commands
            services.AddScoped<IBotUICommand, RequestSearchCommand>();
            services.AddScoped<IBotCommand, RunSearchCommand>();
            services.AddScoped<IBotCommand, CancelSearchCommand>();
            services.AddScoped<IBotCommand, NextSearchResultsCommand>();
            #endregion
        }
    }
}
