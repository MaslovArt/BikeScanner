using System.Text;
using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    public class GetSubsCommand : CommandBase
    {
        private readonly ISubscriptionsService _subsService;

        public GetSubsCommand(ISubscriptionsService subsService)
        {
            _subsService = subsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.Command(CommandNames.UI.UserSubs);

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var userSubs = await _subsService.GetActiveSubs(userId);

            if (userSubs.Length == 0)
            {
                await SendMessage("Нет подписок", context);
                return;
            }

            var message = new StringBuilder($"Всего подписок: {userSubs.Length}");
            for (int i = 0; i < userSubs.Length; i++)
                message.AppendLine($"\n{i + 1}) {userSubs[i].SearchQuery}");
            await SendMessage(message.ToString(), context);
        }
    }
}

