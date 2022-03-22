using System.Text;
using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    public class GetSubsCommand : CommandBase
    {
        private readonly ISubscriptionsService _subsService;

        public GetSubsCommand(ISubscriptionsService subsService)
        {
            _subsService = subsService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.MySubs),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.MySubs));

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var userSubs = await _subsService.GetActiveSubs(userId);

            if (userSubs.Length == 0)
            {
                var addSubBtn = TelegramMarkupHelper.MessageRowBtns(
                    ("Добавить подписку", CommandNames.UI.AddSub));
                await SendMessageWithButtons("Нет подписок", context, addSubBtn);
                return;
            }

            var message = new StringBuilder($"Всего подписок: {userSubs.Length}");
            for (int i = 0; i < userSubs.Length; i++)
                message.AppendLine($"\n{i + 1}) {userSubs[i].SearchQuery}");
            var btns = TelegramMarkupHelper.MessageRowBtns(
                ("Добавить подписку", CommandNames.UI.AddSub),
                ("Удалить подписку", CommandNames.UI.DeleteSub));
            await SendMessageWithButtons(message.ToString(), context, btns);
        }
    }
}

