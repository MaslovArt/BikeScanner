using BikeScanner.Domain.Repositories;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using BikeScanner.UI.Bot.Helpers;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Feed
{
    public class GetFeedCommand : BotUICommand
    {
        private readonly int                    _pageSize;
        private readonly IContentsRepository    _contents;

        public override string CallName => UICommands.ScrollFeed;
        public override string Description => "Лента объявлений";

        public GetFeedCommand(
            IContentsRepository contentsRepository,
            IOptions<TelegramUIConfig> options
            )
        {
            _pageSize = options.Value.SearchResultPageSize;
            _contents = contentsRepository;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var results = await _contents.Scroll(0, _pageSize);
            foreach (var result in results)
            {
                await SendMessage(result.AdUrl, context);
            }

            if (results.Length == _pageSize)
            {
                await SendMessageWithButtons($"Показать еще?", context, TelegramButtonsHelper.BooleanButtons);

                return ContinueWith.Command<MoreFeedCommand>(results.Last().Id);
            }

            return null;
        }
    }
}
