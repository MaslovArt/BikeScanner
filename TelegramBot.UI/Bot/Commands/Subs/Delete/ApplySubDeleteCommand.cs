using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;

namespace TelegramBot.UI.Bot.Commands.Subs
{
	public class ApplySubDeleteCommand : CommandBase
	{
        private readonly ISubscriptionsService _subs;

        public ApplySubDeleteCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.ApplyDeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var subId = int.Parse(GetParam(context, 0, CommandNames.Internal.ApplyDeleteSub));
            var sub = await _subs.RemoveSub(subId);

            var message = $"Поиск '{sub.SearchQuery}' удален.";
            await SendMessage(message, context);
        }
    }
}

