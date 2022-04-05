using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Models.Search;
using BikeScanner.Application.Services.SearchService;
using Microsoft.Extensions.Options;
using TelegramBot.UI.Bot.Helpers;
using TelegramBot.UI.Config;

namespace TelegramBot.UI.Bot.Commands.Search
{
    /// <summary>
    /// Search. Show search results.
    /// </summary>
    public class SearchResultsCommand : CommandBase
    {
        private readonly int _perPage;
        private readonly ISearchService _searchService;

        public SearchResultsCommand(
            ISearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
        }


        public override CommandFilter Filter =>
            CombineFilters.Any(
                FilterDefinitions.StateMessage(BotState.WaitSearchInput),
                FilterDefinitions.CallbackCommand(CommandNames.Internal.ShowSubsFromSearch)
                );

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var searchQuery = ChatInput(context, CommandNames.Internal.ShowSubsFromSearch);

            var result = await _searchService.Search(userId, searchQuery, 0, _perPage);

            var resultMessage = $"По запросу '{searchQuery}' нашел {result.Total} объявлений.";
            var saveSearchBtn = TelegramMarkupHelper.MessageRowBtns(
                ("Сохранить поиск", $"{CommandNames.Internal.AddSubFromSearch} {searchQuery}"));
            await SendMessage(resultMessage, context, saveSearchBtn);

            var adUrls = result.Items.Select(r => r.AdUrl);
            await SendMessages(adUrls, context);

            if (result.Total > result.Items.Length)
            {
                var moreMessage = $"Показать еще? ({result.Total - result.Items.Length})";
                var moreButton = TelegramMarkupHelper.MessageRowBtns(
                    ("Еще", $"{CommandNames.Internal.MoreSearchResults} {searchQuery}{ParamSeparator}{_perPage}"));
                await SendMessage(moreMessage, context, moreButton);
            }

            context.BotContext.State = BotState.Default;
        }
    }
}

