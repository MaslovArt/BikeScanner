using System.Threading.Tasks;
using BikeScanner.UI.Bot.Helpers;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public abstract class ShowMoreCommand : BotCommand
    {
        public override bool ExecuteImmediately => false;

        public abstract Task<ContinueWith> OnMore(CommandContext context);

        public abstract Task<ContinueWith> OnCancel(CommandContext context);

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var input = GetChatInput(context);

            if (input == TelegramButtonsHelper.AcceptButtonValue)
                return await OnMore(context);
            if (input == TelegramButtonsHelper.CancelButtonValue)
                return await OnCancel(context);

            return ContinueWith.Command<ShowMoreCommand>();
        }
    }
}
