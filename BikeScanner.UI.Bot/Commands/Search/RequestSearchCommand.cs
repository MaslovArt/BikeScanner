using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class RequestSearchCommand : BotUICommand
    {
        public override string CallName => UICommands.Search;
        public override string Description => "Запуск поиска";

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            await SendMessage("Что найти?", context);

            return ContinueWith.Command<RunSearchCommand>();
        }
    }
}
