using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    public class ApplySubAddCommand : CommandBase
    {
        private readonly ISubscriptionsService _subs;

        public ApplySubAddCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.StateMessage(BotState.WaitNewSubInput),
            FilterDefinitions.CallbackCommand(CommandNames.Internal.AddSubFromSearch));

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var searchQuery = GetParam(context, 0, CommandNames.Internal.AddSubFromSearch);

            await _subs.AddSub(userId, searchQuery);

            var message = $"Поиск '{searchQuery}' сохранен в подписках.";

            if (context.Update.Type == UpdateType.CallbackQuery)
                await AnswerCallback(message, context);
            else
                await SendMessage(message, context);

            context.BotContext.State = BotState.Default;
        }
    }
}

