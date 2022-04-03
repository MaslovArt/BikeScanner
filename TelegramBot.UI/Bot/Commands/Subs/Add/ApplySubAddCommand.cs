using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.Domain.Repositories;
using Telegram.Bot.Types.Enums;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    /// <summary>
    /// Add new subscription. Save user input to subs.
    /// </summary>
    public class ApplySubAddCommand : CommandBase
    {
        private readonly ISubscriptionsService  _subsService;
        private readonly IContentsRepository    _contentsRepository;

        public ApplySubAddCommand(
            ISubscriptionsService subscriptionsService,
            IContentsRepository contentsRepository
            )
        {
            _subsService = subscriptionsService;
            _contentsRepository = contentsRepository;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.StateMessage(BotState.WaitNewSubInput),
            FilterDefinitions.CallbackCommand(CommandNames.Internal.AddSubFromSearch));

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var searchQuery = ChatInput(context, CommandNames.Internal.AddSubFromSearch);

            await _subsService.AddSub(userId, searchQuery);

            var message = $"Поиск '{searchQuery}' сохранен в подписках.";

            if (context.Update.Type == UpdateType.CallbackQuery)
                await AnswerCallback(message, context);
            else
            {
                await SendMessage(message, context);
                await TryFindAdsByNewSub(searchQuery, context);
            }

            context.BotContext.State = BotState.Default;
        }

        private async Task TryFindAdsByNewSub(string searchQuery, CommandContext context)
        {
            var count = await _contentsRepository.CountSearch(searchQuery);
            if (count > 0)
            {
                var message = $"По новой подписке '{searchQuery}' уже есть {count} объявлений.";
                var showBtn = TelegramMarkupHelper.MessageRowBtns(
                    ("Посмотреть", $"{CommandNames.Internal.ShowSubsFromSearch} {searchQuery}")
                    );

                await SendMessageWithButtons(message, context, showBtn);
            }
        }
    }
}

