using BikeScanner.Application.Interfaces;
using BikeScanner.Infrastructure.Notificators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.UI.Bot.Commands;
using TelegramBot.UI.Config;
using TelegramBot.UI.Bot.Context;
using TelegramBot.UI.Hosting;
using TelegramBot.UI.Bot;
using TelegramBot.UI.Bot.Commands.Main;
using TelegramBot.UI.Bot.Commands.Search;
using TelegramBot.UI.Bot.Commands.Subs;

namespace TelegramBot.UI.DI
{
    public static class ServicesCollectionExtension
	{
		public static IServiceCollection AddBikeScannerBot(
			this IServiceCollection services,
			IConfiguration configuration
			)
        {
			services.Configure<TelegramApiAccessConfig>(configuration.GetSection(nameof(TelegramApiAccessConfig)));
			services.Configure<TelegramUIConfig>(configuration.GetSection(nameof(TelegramUIConfig)));

			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => {
				var bot = x.GetRequiredService<IOptions<TelegramApiAccessConfig>>().Value;
				return new TelegramBotClient(bot.Key);
			});
			services.AddSingleton<INotificator, TelegramNotificator>();

			services.AddScoped<IBotContextService, InMemoryBotContext>();
			services.AddSingleton<BikeScannerBot>();

            #region Main commands
            services.AddTransient<ICommandBase, StartCommand>();
			services.AddTransient<ICommandBase, HelpCommand>();
			services.AddTransient<ICommandBase, UserBlockBotCommand>();
            #endregion
            #region Search commands
            services.AddTransient<ICommandBase, SearchCommand>();
			services.AddTransient<ICommandBase, SearchResultsCommand>();
			services.AddTransient<ICommandBase, MoreSearchResultsCommand>();
			services.AddTransient<ICommandBase, SaveSearchCommand>();
			#endregion
			#region Subs commands
			services.AddTransient<ICommandBase, GetSubsCommand>();
			services.AddTransient<ICommandBase, WhatSubDeleteCommand>();
			services.AddTransient<ICommandBase, ConfirmSubDeleteCommand>();
			services.AddTransient<ICommandBase, ApplySubDeleteCommand>();
			#endregion
			services.AddTransient<ICommandBase, UnknownCommand>(); //always last

			return services;
        }

		public static IServiceCollection AddTelegramWebhook(this IServiceCollection services)
        {
			services.AddHostedService<TelegramWebhookHostedService>();
			return services;
        }

		public static IServiceCollection AddTelegramPolling(this IServiceCollection services)
        {
			services.AddHostedService<TelegramPollHostedService>();
			return services;
        }
	}
}

