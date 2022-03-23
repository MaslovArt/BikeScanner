using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.DevMessage
{
    /// <summary>
    /// Send message to admin. Receive user input.
    /// </summary>
    public class CreateMessageCommand : CommandBase
    {
        public override CommandFilter Filter =>
            FilterDefinitions.StateMessage(BotState.WaitDevMessageInput);

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.Default;
            //... save message
            return SendMessage("Сообщение отправлено.", context);
        }
    }
}

