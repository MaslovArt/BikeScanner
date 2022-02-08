using BikeScanner.Domain.Exceptions;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.BotService.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.BotService.CommandsHandler
{
    public class CommandsHandler
    {
        private readonly IBotCommand[] _commands;
        private readonly IUnknownCommand _unknownCommand;
        private readonly IBotContext _context;
        private readonly ILogger<CommandsHandler> _logger;

        public CommandsHandler(
            IBotContext context,
            IStartBotCommand startCommand,
            IHelpBotCommand helpCommand,
            ICancelCommand cancelCommand,
            IUnknownCommand unknownCommand,
            IEnumerable<IBotCommand> commands,
            IEnumerable<IBotUICommand> uiCommands,
            ILogger<CommandsHandler> logger)
        {
            _unknownCommand = unknownCommand;
            _logger = logger;
            _context = context;
            _commands = commands
                .Union(uiCommands)
                .Append(startCommand)
                .Append(helpCommand)
                .Append(cancelCommand)
                .ToArray();
        }

        public async Task Handle(Update update, ITelegramBotClient client)
        {
            var userId = GetChatId(update);
            var input = GetMessage(update);

            if (string.IsNullOrEmpty(input)) return;

            var userContext = await _context.EnsureContext(userId);

            var command = _commands
                            .OfType<IBotUICommand>()
                            .FirstOrDefault(c => c.CanHandel(input)) ??
                            _commands
                            .FirstOrDefault(c => c.Key == userContext.NextCommand) ??
                            _unknownCommand;

            if (command is IBotUICommand &&
                command is not ICancelCommand &&
                !string.IsNullOrEmpty(userContext.NextCommand))
            {
                var message = $"Сначала нужно завершить или отменить выполняемую командку!";
                await client.SendTextMessageAsync(userId, message);
                return;
            }

            await ExecuteCommandChain(command, userContext, update, client);
        }

        private long GetChatId(Update update)
        {
            return update.Message?.Chat.Id ?? update.CallbackQuery.Message.Chat.Id;
        }
        private string GetMessage(Update update)
        {
            return update.Message?.Text ?? update.CallbackQuery.Data;
        }

        private async Task ExecuteCommandChain(
            IBotCommand command,
            BotContext userContext,
            Update update,
            ITelegramBotClient client)
        {
            var chatId = GetChatId(update);
            try
            {
                _logger.LogDebug($"User[{chatId}] start [{command.Key}]");
                var commandContext = new CommandContext(update, client, userContext.State);
                var continueWith = await command.Execute(commandContext);
                _logger.LogDebug($"User[{chatId}] end [{command.Key}]");

                if (continueWith != null)
                {
                    _logger.LogDebug($"User[{chatId}] next [{continueWith.Key}]");
                    var next = _commands.FirstOrDefault(c => c.Key == continueWith.Key);
                    if (next.ExecuteImmediately)
                    {
                        await ExecuteCommandChain(next, userContext, update, client);
                    }
                    else
                    {
                        userContext.NextCommand = continueWith.Key;
                        userContext.State = continueWith.State;
                        await _context.Update(userContext);
                        _logger.LogDebug($"Command [{next.Key}] wait user[{chatId}]");
                    }
                }
                else if (!string.IsNullOrEmpty(userContext.NextCommand))
                {
                    userContext.NextCommand = null;
                    await _context.Update(userContext);
                    _logger.LogDebug($"User[{chatId}] completed task");
                }
            }
            catch (Exception ex)
            {
                await HandleError(chatId, client, ex);
            }
        }

        private Task HandleError(long chatId, ITelegramBotClient client, Exception ex)
        {
            if (ex is AppError)
            {
                return client.SendTextMessageAsync(chatId, $"Упс. {ex.Message}");
            }
            else
            {
                _logger.LogError(ex, $"User[{chatId}] error:{ex.Message}");
                return client.SendTextMessageAsync(chatId, "Что-то пошло не так(");
            }
        }
    }
}
