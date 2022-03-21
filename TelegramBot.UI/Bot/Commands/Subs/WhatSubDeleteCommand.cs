using System.Linq;
using System.Threading.Tasks;
using BikeScanner.Application.Services.SubscriptionsService;
using TelegramBot.UI.Bot.Filters;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Subs
{
    public class WhatSubDeleteCommand : CommandBase
    {
        private readonly ISubscriptionsService _subs;

        public WhatSubDeleteCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.Command(CommandNames.UI.DeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var userId = UserId(context);
            var userSubs = await _subs.GetActiveSubs(userId);

            if (userSubs.Length == 0)
            {
                await SendMessage("Удалять нечего. Подписок нет.", context);
                return;
            }

            var btns = userSubs
                .Select(s => (s.SearchQuery, $"{CommandNames.Internal.ConfirmDeleteSub} {s.Id}"))
                .ToArray();
            await SendMessageWithButtons("Какой удалить?", context, TelegramMarkupHelper.MessageColumnBtns(btns));
        }
    }
}

