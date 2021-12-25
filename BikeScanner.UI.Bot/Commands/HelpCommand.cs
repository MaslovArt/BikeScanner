using BikeScanner.UI.Bot.BotService.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Return all available ui commands with description
    /// </summary>
    public class HelpCommand : BotUICommand, IHelpBotCommand
    {
        private readonly IBotUICommand[] _commands;

        public override string CallName => UICommands.Help;
        public override string Description => "Возможности";

        public HelpCommand(IEnumerable<IBotUICommand> commands)
        {
            _commands = commands.ToArray();
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var reply = new StringBuilder();
            reply.AppendLine($"Возможности бота. ({_commands.Length})\n");
            foreach (var command in _commands)
            {
                reply.AppendLine($"{command.CallName} - {command.Description}");
            }

            await SendMessage(reply.ToString(), context);

            return null;
        }
    }
}
