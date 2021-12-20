using BikeScanner.UI.Bot.BotService.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Return all available ui commands with description
    /// </summary>
    public class HelpCommand : BotUICommand, IHelpBotCommand
    {
        private readonly IBotUICommand[] _commands;

        public override string CallName => UICommands.Help;
        public override string Description => "Помощь";
        public override string CancelWith => null;

        public HelpCommand(
            IEnumerable<IBotUICommand> commands,
            ICancelCommand cancelCommand)
        {
            _commands = commands
                .Append(cancelCommand)
                .ToArray();
        }

        public override async Task<string> Execute(Update update, ITelegramBotClient client)
        {
            var reply = new StringBuilder();
            reply.AppendLine($"Возможности бота. ({_commands.Length})\n");
            foreach (var command in _commands)
            {
                reply.AppendLine($"{command.CallName} - {command.Description}");
            }

            await SendMessage(reply.ToString(), update, client);

            return null;
        }
    }
}
