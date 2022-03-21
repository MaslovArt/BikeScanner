using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Search
{
    public class SaveSearchCommand : CommandBase
    {
        private readonly ISubscriptionsService _subs;

        public SaveSearchCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.SaveSearch);

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var searchQuery = GetParam(context, 0, CommandNames.Internal.SaveSearch);

            await _subs.AddSub(userId, searchQuery);

            var message = $"Поиск '{searchQuery}' сохранен в подписках.";
            await AnswerCallback(message, context);
        }
    }
}

