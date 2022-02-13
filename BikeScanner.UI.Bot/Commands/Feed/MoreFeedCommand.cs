using BikeScanner.Domain.Repositories;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Configs;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Feed
{
    public class MoreFeedCommand : ShowMoreCommand
    {
        private readonly int _pageSize;
        private readonly IContentsRepository _contents;

        public override CancelWith CancelWith => null;

        public MoreFeedCommand(
            IContentsRepository contentsRepository,
            IOptions<TelegramUIConfig> options
            )
        {
            _pageSize = options.Value.SearchResultPageSize;
            _contents = contentsRepository;
        }

        public async override Task<ContinueWith> OnCancel(CommandContext context)
        {
            await SendMessage("Завершил показ ленты", context);

            return null;
        }

        public async override Task<ContinueWith> OnMore(CommandContext context)
        {
            var takeAfter = context.GetState<int>();

            var results = await _contents.Scroll(takeAfter, _pageSize);
            foreach (var result in results)
            {
                await SendMessage(result.AdUrl, context);
            }

            if (results.Length == _pageSize)
            {
                var message = $"Показать еще?";
                await SendMessageColumnButtons(message, context, Buttons);

                return ContinueWith.Command<MoreFeedCommand>(results.Last().Id);
            }

            return null;
        }
    }
}
