using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class ApplySubDelCommand : ConfirmCommand
    {
        private readonly ISubscriptionsService _subs;

        public override string CancelWith => nameof(CancelSubDelCommand);

        public ApplySubDelCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public async override Task<ContinueWith> OnConfirm(CommandContext context)
        {
            var deletingSubId = context.GetState<int>();
            var deletedSub = await _subs.RemoveSub(deletingSubId);

            await SendMessage($"Подписка '{deletedSub.SearchQuery} удалена.", context);

            return null;
        }

        public override Task<ContinueWith> OnCancel(CommandContext context)
        {
            return Task.FromResult(ContinueWith.Command<CancelSubDelCommand>());
        }
    }
}
