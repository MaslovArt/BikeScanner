using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Helpers;
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
            var message = $"Не понимаю что мне делать(";
            var getHelpBtn = TelegramMarkupHelper.MessageRowBtns(("Посмотреть возможности", UICommands.Help));
            await SendMessageWithButtons(message, context, getHelpBtn);

            return null;
        }
    }
}
