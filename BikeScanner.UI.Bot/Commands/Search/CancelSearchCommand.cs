using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class CancelSearchCommand : BotCommand
    {
        public override bool ExecuteImmediately => true;
        public override string CancelWith => null;

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            await SendMessage("Ничего. Понял", context);

            return null;
        }
    }
}
