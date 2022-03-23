using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    /// <summary>
    /// Add new subscription. Ask what to add.
    /// </summary>
    public class AddSubCommand : CommandBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.AddSub),
            FilterDefinitions.CallbackCommand(CommandNames.UI.AddSub));

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.WaitNewSubInput;

            var questionMsg = @"Какой поиск добавить в подписку?
Отправлю уведомление, когда появится похожее объявление.";
            return SendMessage(questionMsg, context);
        }
    }
}

