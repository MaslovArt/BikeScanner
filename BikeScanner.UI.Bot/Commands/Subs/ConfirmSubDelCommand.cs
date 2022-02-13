using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class ConfirmSubDelCommand : BotCommand
    {
        private readonly ISubscriptionsService _subs;

        public override bool ExecuteImmediately => false;
        public override CancelWith CancelWith => CancelWith.Command<CancelSubDelCommand>();

        public ConfirmSubDelCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetChatId(context);
            var input = GetChatInput(context);

            var userSubs = await _subs.GetActiveSubs(chatId);
            var deletingSub = userSubs.SingleOrDefault(s => s.SearchQuery == input);

            if (deletingSub == null)
                ContinueWith.Command<ConfirmSubDelCommand>();

            var message = $"Удалить '{deletingSub.SearchQuery}'?";
            await SendMessageRowButtons(message, context, ConfirmCommand.ConfirmButtons);

            return ContinueWith.Command<ApplySubDelCommand>(deletingSub.Id);
        }
    }
}
