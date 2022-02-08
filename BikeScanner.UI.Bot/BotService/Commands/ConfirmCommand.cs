using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public abstract class ConfirmCommand : BotCommand
    {
        public static string[] ConfirmButtons = new string[]
        {
            "Да",
            "Нет"
        };
        public override bool ExecuteImmediately => false;

        public abstract Task<ContinueWith> OnConfirm(CommandContext context);

        public abstract Task<ContinueWith> OnCancel(CommandContext context);

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var input = GetChatInput(context);

            if (input == "Да") 
                return await OnConfirm(context);
            if (input == "Нет") 
                return await OnCancel(context);

            return ContinueWith.Command<ConfirmCommand>();
        }
    }
}
