using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class CancelSearchCommand : BotCommand
    {
        public override bool ExecuteImmediately => true;
        public override CancelWith CancelWith => null;

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            await SendMessage("Отменил поиск.", context);

            return null;
        }
    }
}
