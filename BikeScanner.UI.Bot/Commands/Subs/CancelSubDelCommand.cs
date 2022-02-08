using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class CancelSubDelCommand : BotCommand
    {
        public override bool ExecuteImmediately => true;
        public override string CancelWith => null;

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            await SendMessage("Отменил удаление подписки.", context);

            return null;
        }
    }
}
