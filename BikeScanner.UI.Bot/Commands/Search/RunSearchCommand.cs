using BikeScanner.Application.Services.SearchService;
using BikeScanner.Domain.Repositories;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class RunSearchCommand : BotCommand
    {
        private readonly int                        _perPage;
        private readonly ISearchService             _searchService;
        private readonly ISearchHistoryRepository   _searchHistory;

        public override bool ExecuteImmediately => false;
        public override string CancelWith => nameof(CancelSearchCommand);

        public RunSearchCommand(
            ISearchService searchService, 
            ISearchHistoryRepository searchHistoryRepository,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
            _searchHistory = searchHistoryRepository;
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetChatId(context);
            var input = GetChatInput(context);

            await _searchHistory.WriteHistory(chatId, input);

            var results = await _searchService.Search(chatId, input, 0, _perPage);

            await SendMessage($"Нашел {results.Total} объявлений", context);
            foreach (var result in results.Items)
            {
                await SendMessage(result.AdUrl, context);
            }

            if (results.Total > results.Items.Length)
            {
                var state = new SearchState(input, _perPage);
                var message = $"Показать еще ({results.Total - results.Items.Length})?";
                var btns = new string[]
                {
                    SearchConstants.COMPLETE_BTN,
                    SearchConstants.NEXT_BTN
                };
                await SendMessageColumnButtons(message, context, btns);

                return ContinueWith.Command<NextSearchResultsCommand>(state);
            }

            return null;
        }
    }
}
