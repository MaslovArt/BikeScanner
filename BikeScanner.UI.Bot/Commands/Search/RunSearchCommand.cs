using BikeScanner.Application.Services.SearchService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class RunSearchCommand : BotCommand
    {
        private readonly int               _perPage;
        private readonly ISearchService    _searchService;

        public override bool ExecuteImmediately => false;
        public override CancelWith CancelWith => CancelWith.Command<CancelSearchCommand>();

        public RunSearchCommand(
            ISearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetChatId(context);
            var input = GetChatInput(context);

            var results = await _searchService.Search(chatId, input, 0, _perPage);

            var resultMessage = $"Нашел {results.Total} объявлений.\n(Сохранить поиск {UICommands.SaveSearch})";
            await SendMessage(resultMessage, context);
            foreach (var result in results.Items)
            {
                await SendMessage(result.AdUrl, context);
            }

            if (results.Total > results.Items.Length)
            {
                var askMoreMessage = $"Показать еще ({results.Total - results.Items.Length})?";
                await SendMessageColumnButtons(askMoreMessage, context, ShowMoreCommand.Buttons);

                var state = new SearchState()
                {
                    SearchQuery = input,
                    Skip = results.Offset
                };
                return ContinueWith.Command<MoreSearchResultsCommand>(state);
            }

            return null;
        }
    }
}
