using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.BotService.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public CancelCommand(IEnumerable<IBotCommand> commands, IBotContext botContext)
        {
            _commands = commands.ToArray();
            _botContext = botContext;
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetUserId(context);
            var userContext = await _botContext.GetUserContext(chatId);
            var cancelingCommand = userContext?.NextCommand;

            if (string.IsNullOrEmpty(cancelingCommand))
            {
                var message = "Хм..\nА что отменять то? Я ничего не делаю.";
                await SendMessage(message, context);
            }
            else
            {
                var currentCommand = _commands.FirstOrDefault(c => c.Key == cancelingCommand);
                if (currentCommand?.CancelWith != null)
                {
                    return ContinueWith.Command(currentCommand.CancelWith.Key);
                }
                else
                {
                    await SendMessage("Отменил.", context);
                }
            }

            return null;
        }
    }
}
