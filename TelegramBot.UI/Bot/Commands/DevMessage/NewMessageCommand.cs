using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.DevMessage
{
    /// <summary>
    /// Send message to admin. Ask what to send.
    /// </summary>
    public class NewMessageCommand : CommandBase
    {
        public override CommandFilter Filter =>
            FilterDefinitions.UICommand(CommandNames.UI.DevMessage);

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.WaitDevMessageInput;

            var question = @"Какое сообщение отправить разработчику?";
            return SendMessage(question, context);
        }
    }
}

