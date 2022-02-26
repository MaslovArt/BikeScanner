using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Application.Types;
using BikeScanner.Domain.Extentions;
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
            var chatId = GetChatId(context);
            var searchQueryParam = GetParam(context, 0);

            var newSub = new Subscription()
            {
                UserId = chatId,
                SearchQuery = searchQueryParam,
                SubscriptionType = "Telegram"
            };
            await _subs.AddSub(newSub);

            var message = $"Поиск '{searchQueryParam}' сохранен в подписках.";
            var getSubsBtn = TelegramMarkupHelper.MessageRowBtns(("Мои подписки", UICommands.UserSubs));
            await SendMessageWithButtons(message, context, getSubsBtn);

            return null;
        }
    }
}
