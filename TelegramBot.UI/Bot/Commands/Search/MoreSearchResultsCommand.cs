using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Services.SearchService;
using Microsoft.Extensions.Options;
using TelegramBot.UI.Bot.Helpers;
using TelegramBot.UI.Config;

namespace TelegramBot.UI.Bot.Commands.Search
{
    public class MoreSearchResultsCommand : CommandBase
    {
        private readonly int _perPage;
        private readonly ISearchService _searchService;

        public MoreSearchResultsCommand(
            ISearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.MoreSearchResults);

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var searchQuery = GetParam(context, 0, CommandNames.Internal.MoreSearchResults);
            var skip = int.Parse(GetParam(context, 1, CommandNames.Internal.MoreSearchResults));

            var result = await _searchService.Search(userId, searchQuery, skip, _perPage);

            var urls = result.Entities.Select(r => r.AdUrl);
            await SendMessages(urls, context);

            if (result.Total > result.Offset)
            {
                var moreMessage = $"Показать еще? ({result.Total - result.Offset})";
                var moreButton = TelegramMarkupHelper.MessageRowBtns(
                    ("Еще", $"{CommandNames.Internal.MoreSearchResults} {searchQuery};{skip + _perPage}"));
                await SendMessageWithButtons(moreMessage, context, moreButton);
            }
        }
    }
}

