using System.Threading.Tasks;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;

namespace TelegramBot.UI.Bot.Commands.DevMessage
{
    /// <summary>
    /// Send message to admin. Receive user input.
    /// </summary>
    public class ApplySendMessageCommand : CommandBase
    {
        private readonly IDevMessagesRepository _devMessagesRepository;

        public ApplySendMessageCommand(IDevMessagesRepository devMessagesRepository)
        {
            _devMessagesRepository = devMessagesRepository;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.StateMessage(BotState.WaitDevMessageInput);

        public override async Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.Default;

            var userId = UserId(context);
            var input = ChatInput(context);

            var newMsg = new DevMessageEntity(userId, input);
            await _devMessagesRepository.Add(newMsg);

            await SendMessage("Сообщение отправлено.", context);
        }
    }
}

