using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Delete selected sub.
    /// </summary>
	public class ApplySubDeleteCommand : GetSubsCommand
	{
        public ApplySubDeleteCommand(ISubscriptionsService subscriptionsService)
            : base(subscriptionsService)
        { }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.ApplyDeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var subId = int.Parse(ChatInput(context, CommandNames.Internal.ApplyDeleteSub));
            var deletedSub = await _subsService.RemoveSub(subId);

            var deleteMessage = $"{Emoji.X} Поиск '{deletedSub.SearchQuery}' удален. {Emoji.X}";
            await AnswerCallback(deleteMessage, context);

            await base.Execute(context);
        }
    }
}

