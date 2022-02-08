using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Application.Types;
using BikeScanner.Domain.Exceptions;
using BikeScanner.Domain.Repositories;
using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class SaveLastSearchCommand : BotUICommand
    {
        private readonly ISubscriptionsService      _subs;
        private readonly ISearchHistoryRepository   _searchHistory;

        public override string CancelWith => null;
        public override string CallName => UICommands.SaveSearch;
        public override string Description => "Сохранить последний поиск";

        public SaveLastSearchCommand(
            ISubscriptionsService subscriptionsService, 
            ISearchHistoryRepository searchHistoryRepository
            )
        {
            _subs = subscriptionsService;
            _searchHistory = searchHistoryRepository;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetChatId(context);
            var lastSearch = await _searchHistory.GetLast(chatId);

            if (lastSearch == null)
                throw AppError.NotExists("Поиск для сохранения");

            var newSub = new Subscription()
            {
                UserId = chatId,
                SearchQuery = lastSearch.SearchQuery,
                SubscriptionType = "Telegram"
            };
            await _subs.AddSub(newSub);

            var message = $"Поиск '{lastSearch.SearchQuery}' сохранен в подписках ({UICommands.UserSubs})";
            await SendMessage(message, context);

            return null;
        }
    }
}
