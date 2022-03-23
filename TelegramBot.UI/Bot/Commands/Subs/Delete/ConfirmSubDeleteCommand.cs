using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Confirm user input.
    /// </summary>
    public class ConfirmSubDeleteCommand : CommandBase
    {
        private readonly ISubscriptionsService _subs;

        public ConfirmSubDeleteCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.ConfirmDeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var subId = int.Parse(GetParam(context, 0, CommandNames.Internal.ConfirmDeleteSub));
            var sub = await _subs.GetSub(subId);

            var confirmMessage = $"Подтвердите удаление '{sub.SearchQuery}'";
            var confirmBtn = TelegramMarkupHelper.MessageRowBtns(
                ("Удалить", $"{CommandNames.Internal.ApplyDeleteSub} {sub.Id}"));
            await SendMessageWithButtons(confirmMessage, context, confirmBtn);
        }
    }
}
