using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Ask what to delete.
    /// </summary>
    public class DeleteSubCommand : CommandUIBase
    {
        private readonly ISubscriptionsService _subs;

        public DeleteSubCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.DeleteSub),
            FilterDefinitions.CallbackCommand(CommandNames.UI.DeleteSub));

        public override async Task ExecuteCommand(CommandContext context)
        {
            var userId = UserId(context);
            var userSubs = await _subs.GetSubs(userId);

            if (userSubs.Length == 0)
            {
                await SendMessage("Удалять нечего. Подписок нет.", context);
                return;
            }

            var btns = userSubs
                .Select(s => (s.SearchQuery, $"{CommandNames.Internal.ConfirmDeleteSub} {s.Id}"))
                .Append(BaseButtons.Cancel)
                .ToArray();
            await EditCallbackMessage("Какой удалить?", context, TelegramMarkupHelper.MessageColumnBtns(btns));

            context.BotContext.State = BotState.DeleteSub;
        }
    }
}

