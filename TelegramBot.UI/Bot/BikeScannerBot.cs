using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.UI.Bot.Commands;
using TelegramBot.UI.Bot.Context;
using TelegramBot.UI.Exceptions;

namespace TelegramBot.UI.Bot
{
	public class BikeScannerBot
	{
		record HandlerRule(Type Type, CommandFilter Predicate);

		private readonly ILogger<BikeScannerBot> _logger;
		private readonly HandlerRule[] _rules;
		private readonly IServiceProvider _provider;
		private readonly ITelegramBotClient _client;

		public BikeScannerBot(
			IServiceProvider serviceProvider,
			ILogger<BikeScannerBot> logger,
			ITelegramBotClient telegramBotClient
			)
		{
			_logger = logger;
			_provider = serviceProvider;
			_client = telegramBotClient;

			using var scope = _provider.CreateScope();
			var commands = scope
				.ServiceProvider
				.GetRequiredService<IEnumerable<ICommandBase>>();
			_rules = commands
				.Select(c => new HandlerRule(c.GetType(), c.Filter))
				.ToArray();
		}

        public async Task Handle(Update update)
        {
			if (update == null) return;
			try
			{
				using var scope = _provider.CreateScope();
				var contextService = scope.ServiceProvider.GetRequiredService<IBotContextService>();
				var chatId = GetChatId(update);
				var context = await contextService.EnsureContext(chatId);
				var rule = _rules.FirstOrDefault(r => r.Predicate(update, context)) ??
					throw new UpdateHandlerException(update, context);

				_logger.LogInformation($"User[{chatId}] exec [{rule.Type.Name}]");

				var handler = ActivatorUtilities.CreateInstance(scope.ServiceProvider, rule.Type) as ICommandBase;
				var commandContext = new CommandContext(update, _client, context);
				await handler.Execute(commandContext);
				await contextService.Update(commandContext.BotContext);
			}
			catch (Exception ex)
            {
				await OnError(update, _client, ex);
            }
        }

		private Task OnError(Update update, ITelegramBotClient client, Exception ex)
        {
			var chatId = GetChatId(update);
			if (ex is AppError)
			{
				return client.SendTextMessageAsync(chatId, ex.Message);
			}
			else
			{
				_logger.LogError(ex, $"User[{chatId}] error:{ex.Message} stackTrace:${ex.StackTrace}");
				return TrySendErrorMessage(client, chatId);
			}
		}

		private Task TrySendErrorMessage(ITelegramBotClient client, long chatId)
        {
			try
            {
				return client.SendTextMessageAsync(chatId, "Что-то пошло не так(");
			}
			catch (Exception ex)
            {
				_logger.LogError(ex, $"User[{chatId}] onError err:{ex.Message}");
				return Task.CompletedTask;
			}
        }

		private long GetChatId(Update update)
        {
			return
				update.Message?.Chat?.Id ??
				update.CallbackQuery?.Message?.Chat?.Id ??
				update.MyChatMember?.Chat?.Id ??
				throw new ChatIdException(update);
        }
    }
}

