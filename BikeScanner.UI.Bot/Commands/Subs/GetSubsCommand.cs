using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class GetSubsCommand : BotUICommand
    {
        private readonly ISubscriptionsService _subsService;

        public override string CallName => UICommands.UserSubs;
        public override string Description => "Посмотреть мои подписки";

        public GetSubsCommand(ISubscriptionsService subscriptionsService)
        {
            _subsService = subscriptionsService;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetUserId(context);
            var userSubs = await _subsService.GetActiveSubs(chatId);

            if (userSubs.Length == 0)
            {
                await SendMessage("Нет подписок", context);

                return null;
            }

            var message = new StringBuilder($"Всего подписок: {userSubs.Length}");
            for (int i = 0; i < userSubs.Length; i++)
            {
                message.AppendLine($"\n{i + 1}) {userSubs[i].SearchQuery}");
            }

            await SendMessage(message.ToString(), context);

            return null;
        }
    }
}
