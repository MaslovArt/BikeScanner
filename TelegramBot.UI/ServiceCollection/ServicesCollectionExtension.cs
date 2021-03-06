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
using TelegramBot.UI.Bot.Commands.DevMessage;

namespace TelegramBot.UI.ServiceCollection
{
    public static class ServicesCollectionExtension
	{
		public static IServiceCollection AddBikeScannerTelegramBotUI(
			this IServiceCollection services,
			IConfiguration configuration
			)
        {
			services.Configure<TelegramApiAccessConfig>(
				configuration.GetSection(nameof(TelegramApiAccessConfig)));
			services.Configure<TelegramUIConfig>(
				configuration.GetSection(nameof(TelegramUIConfig)));

			services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => {
				var bot = x.GetRequiredService<IOptions<TelegramApiAccessConfig>>().Value;
				return new TelegramBotClient(bot.Key);
			});

			services.AddScoped<IBotContextService, InMemoryBotContext>();
			services.AddSingleton<BikeScannerBot>();

			#region Special commands
			services.AddTransient<ICommandBase, UserBlockBotCommand>();
			services.AddTransient<ICommandBase, UserJoinBotCommand>();
			#endregion
			#region UI Commands
			services.AddTransient<ICommandBase, StartCommand>();
			services.AddTransient<ICommandBase, HelpCommand>();
			services.AddTransient<ICommandBase, SearchCommand>();
			services.AddTransient<ICommandBase, GetSubsCommand>();
			services.AddTransient<ICommandBase, DeleteSubCommand>();
			services.AddTransient<ICommandBase, AddSubCommand>();
			services.AddTransient<ICommandBase, SendMessageCommand>();
			#endregion
            #region Internal commands
			services.AddTransient<ICommandBase, SearchResultsCommand>();
			services.AddTransient<ICommandBase, MoreSearchResultsCommand>();
			services.AddTransient<ICommandBase, ConfirmSubDeleteCommand>();
			services.AddTransient<ICommandBase, ApplySubDeleteCommand>();
			services.AddTransient<ICommandBase, ApplySubAddCommand>();
			services.AddTransient<ICommandBase, ApplySendMessageCommand>();
            #endregion

            services.AddTransient<ICommandBase, UnknownCommand>(); //always last

			return services;
        }

		public static IServiceCollection AddTelegramWebhookHostedService(
			this IServiceCollection services
			)
        {
			services.AddHostedService<TelegramWebhookHostedService>();
			return services;
        }

		public static IServiceCollection AddTelegramPollingHostedService(
			this IServiceCollection services
			)
        {
			services.AddHostedService<TelegramPollHostedService>();
			return services;
        }
	}
}

