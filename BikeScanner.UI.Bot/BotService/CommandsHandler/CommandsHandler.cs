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

            var currentAction = await _context.EnsureContext(userId);

            var command = _commands
                            .OfType<IBotUICommand>()
                            .FirstOrDefault(c => c.CanHandel(input)) ??
                            _commands
                            .FirstOrDefault(c => c.Key == currentAction.NextCommand) ??
                            _unknownCommand;

            if (command is IBotUICommand &&
                command is not ICancelCommand &&
                !string.IsNullOrEmpty(currentAction.NextCommand))
            {
                var message = $"Сначала нужно завершить или отменить выполняемую командку!";
                await client.SendTextMessageAsync(userId, message);
                return;
            }

            await ExecuteCommandChain(command, currentAction, update, client);
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
            BotContextModel currentAction,
            Update update,
            ITelegramBotClient client)
        {
            var userId = GetChatId(update);
            try
            {
                _logger.LogDebug($"User[{userId}] start [{command.Key}]");
                var nextCommand = await command.Execute(update, client);
                _logger.LogDebug($"User[{userId}] end [{command.Key}]");

                if (!string.IsNullOrEmpty(nextCommand))
                {
                    _logger.LogDebug($"User[{userId}] next [{nextCommand}]");
                    var next = _commands.FirstOrDefault(c => c.Key == nextCommand);
                    if (next.ExecuteImmediately)
                    {
                        await ExecuteCommandChain(next, currentAction, update, client);
                    }
                    else
                    {
                        currentAction.NextCommand = next.Key;
                        await _context.Update(currentAction);
                        _logger.LogDebug($"Command [{next.Key}] wait user[{userId}]");
                    }
                }
                else if (!string.IsNullOrEmpty(currentAction.NextCommand))
                {
                    currentAction.NextCommand = null;
                    await _context.Update(currentAction);
                    _logger.LogDebug($"User[{userId}] completed task");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"User[{userId}] error:{ex.Message}");
            }
        }
    }
}
