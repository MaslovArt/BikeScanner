using BikeScanner.Application.Services.SearchService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class NextSearchResultsCommand : BotCommand
    {
        private readonly int            _perPage;
        private readonly ISearchService _searchService;
        private readonly string[]       _buttons;

        public override bool ExecuteImmediately => false;
        public override string CancelWith => null;

        public NextSearchResultsCommand(
            ISearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
            _buttons = new string[] 
            {
                SearchConstants.COMPLETE_BTN,
                SearchConstants.NEXT_BTN
            };
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var state = context.GetState<SearchState>();
            var chatId = GetChatId(context);
            var input = GetChatInput(context);

            if (input == SearchConstants.COMPLETE_BTN)
            {
                await SendMessage("Завершил", context);

                return null;
            }

            if (input == SearchConstants.NEXT_BTN)
            {
                var results = await _searchService.Search(chatId, state.SearchQuery, state.Skip, _perPage);

                foreach (var result in results.Items)
                {
                    await SendMessage(result.AdUrl, context);
                }

                var totalSkip = results.Items.Length + state.Skip;
                if (results.Total > totalSkip)
                {
                    var message = $"Показать еще ({results.Total - totalSkip})?";
                    await SendMessageColumnButtons(message, context, _buttons);

                    var newState = new SearchState(state.SearchQuery, state.Skip + _perPage);

                    return ContinueWith.Command<NextSearchResultsCommand>(newState);
                }

                await SendMessage("На этом все", context);

                return null;
            }

            await SendMessageColumnButtons("Что?", context, _buttons);

            return ContinueWith.Command<NextSearchResultsCommand>(state);
        }
    }
}
