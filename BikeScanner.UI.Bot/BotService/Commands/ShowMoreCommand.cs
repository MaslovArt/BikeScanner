using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public abstract class ShowMoreCommand : BotCommand
    {
        public static string[] Buttons = new string[]
        {
            "Показать еще",
            "Завершить"
        };
        public override bool ExecuteImmediately => false;

        public abstract Task<ContinueWith> OnMore(CommandContext context);

        public abstract Task<ContinueWith> OnCancel(CommandContext context);

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var input = GetChatInput(context);

            if (input == "Показать еще")
                return await OnMore(context);
            if (input == "Завершить")
                return await OnCancel(context);

            return ContinueWith.Command<ShowMoreCommand>();
        }
    }
}
