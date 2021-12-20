using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.BotService.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Cancel current user command
    /// </summary>
    public class CancelCommand : BotUICommand, ICancelCommand
    {
        private readonly IBotCommand[]  _commands;
        private readonly IBotContext    _botContext;

        public override string CallName => UICommands.Cancel;
        public override string Description => "Отмена действия";
        public override string CancelWith => null;

        public CancelCommand(IEnumerable<IBotCommand> commands, IBotContext botContext)
        {
            _commands = commands.ToArray();
            _botContext = botContext;
        }

        public override async Task<string> Execute(Update update, ITelegramBotClient client)
        {
            var chatId = GetChatId(update);
            var userContext = await _botContext.GetUserContext(chatId);
            var cancelingCommand = userContext?.NextCommand;

            if (string.IsNullOrEmpty(cancelingCommand))
            {
                var message = "Хм..\nА что отменять то? Я ничего не делаю.";
                await SendMessage(message, update, client);
            }
            else
            {
                var currentCommand = _commands.FirstOrDefault(c => c.Key == cancelingCommand);
                if (string.IsNullOrEmpty(currentCommand?.CancelWith))
                {
                    await SendMessage("Отменил", update, client);
                }
                else
                {
                    return currentCommand.CancelWith;
                }
            }

            return null;
        }
    }
}
