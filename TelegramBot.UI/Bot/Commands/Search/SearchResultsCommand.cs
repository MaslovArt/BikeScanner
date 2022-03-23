using System.Linq;
using System.Threading.Tasks;
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
            FilterDefinitions.StateMessage(BotState.WaitSearchInput);

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var input = ChatInput(context);

            var result = await _searchService.Search(userId, input, 0, _perPage);

            var resultMessage = $"По запросу '{input}' нашел {result.Total} объявлений.";
            var saveSearchBtn = TelegramMarkupHelper.MessageRowBtns(
                ("Сохранить поиск", $"{CommandNames.Internal.AddSubFromSearch} {input}"));
            await SendMessageWithButtons(resultMessage, context, saveSearchBtn);

            var adUrls = result.Entities.Select(r => r.AdUrl);
            await SendMessages(adUrls, context);

            if (result.Total > result.Entities.Length)
            {
                var moreMessage = $"Показать еще? ({result.Total - result.Entities.Length})";
                var moreButton = TelegramMarkupHelper.MessageRowBtns(
                    ("Еще", $"{CommandNames.Internal.MoreSearchResults} {input}{ParamSeparator}{_perPage}"));
                await SendMessageWithButtons(moreMessage, context, moreButton);
            }

            context.BotContext.State = BotState.Default;
        }
    }
}

