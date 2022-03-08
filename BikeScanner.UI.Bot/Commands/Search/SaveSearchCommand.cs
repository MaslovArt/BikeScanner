using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Helpers;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class SaveSearchCommand : BotUICommand
    {
        private readonly ISubscriptionsService _subs;

        public override string CallName => UICommands.SaveSearch;
        public override string Description => "Сохранить поиск";

        public SaveSearchCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var userId = GetUserId(context);
            var searchQueryParam = GetParam(context, 0);

            await _subs.AddSub(userId, searchQueryParam);

            var message = $"Поиск '{searchQueryParam}' сохранен в подписках.";
            var getSubsBtn = TelegramMarkupHelper.MessageRowBtns(("Мои подписки", UICommands.UserSubs));
            await SendMessageWithButtons(message, context, getSubsBtn);

            return null;
        }
    }
}
