using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Command for unclear situations
    /// </summary>
    public class UnknownCommand : BotCommand, IUnknownCommand
    {
        public override bool ExecuteImmediately => true;
        public override CancelWith CancelWith => null;

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var message = $"И что мне с этим делать?\nМои возможности ({UICommands.Help}).";
            await SendMessage(message, context);

            return null;
        }
    }
}
