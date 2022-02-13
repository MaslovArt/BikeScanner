﻿using BikeScanner.Application.Services.SearchService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Search
{
    public class MoreSearchResultsCommand : ShowMoreCommand
    {
        private readonly int            _perPage;
        private readonly ISearchService _searchService;

        public override bool ExecuteImmediately => false;
        public override CancelWith CancelWith => CancelWith.Command<CancelSearchCommand>();

        public MoreSearchResultsCommand(
            ISearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchResultPageSize;
            _searchService = searchService;
        }

        public async override Task<ContinueWith> OnMore(CommandContext context)
        {
            var state = context.GetState<SearchState>();
            var chatId = GetChatId(context);

            var results = await _searchService.Search(chatId, state.SearchQuery, state.Skip, _perPage);

            foreach (var result in results.Items)
            {
                await SendMessage(result.AdUrl, context);
            }

            if (results.Total > results.Offset)
            {
                var message = $"Показать еще ({results.Total - results.Offset})?";
                await SendMessageColumnButtons(message, context, Buttons);

                var newState = state with { Skip = results.Offset };
                return ContinueWith.Command<MoreSearchResultsCommand>(newState);
            }

            return null;
        }

        public async override Task<ContinueWith> OnCancel(CommandContext context)
        {
            await SendMessage("Завершил показ результатов", context);

            return null;
        }
    }
}
