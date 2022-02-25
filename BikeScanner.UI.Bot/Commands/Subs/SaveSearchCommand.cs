using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Application.Types;
using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BikeScanner.UI.Bot.Commands.Subs
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
            var getSubsBtn = InlineKeyboardButton.WithCallbackData("Мои подписки", $"{UICommands.UserSubs}");
            await SendMessageRowButtons(message, context, getSubsBtn);

            return null;
        }
    }
}
