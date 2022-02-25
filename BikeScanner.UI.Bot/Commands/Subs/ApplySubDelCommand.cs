using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Helpers;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class ApplySubDelCommand : ConfirmCommand
    {
        private readonly ISubscriptionsService _subs;

        public override CancelWith CancelWith => CancelWith.Command<CancelSubDelCommand>();

        public ApplySubDelCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public async override Task<ContinueWith> OnConfirm(CommandContext context)
        {
            var deletingSubId = context.GetState<int>();
            var deletedSub = await _subs.RemoveSub(deletingSubId);

            var message = $"Подписка '{deletedSub.SearchQuery} удалена.";
            var getSubsBtn = TelegramMarkupHelper.MessageRowBtns(("Мои подписки", UICommands.UserSubs));
            await SendMessageWithButtons(message, context, getSubsBtn);

            return null;
        }

        public override Task<ContinueWith> OnCancel(CommandContext context)
        {
            return Task.FromResult(ContinueWith.Command<CancelSubDelCommand>());
        }
    }
}
