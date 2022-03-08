using BikeScanner.Application.Services.SubscriptionsService;
using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands.Subs
{
    public class WhatSubDelCommand : BotUICommand
    {
        private readonly ISubscriptionsService _subs;

        public override string CallName => UICommands.DeleteSub;
        public override string Description => "Удалить сохраненный поиск";

        public WhatSubDelCommand(ISubscriptionsService subscriptionsService)
        {
            _subs = subscriptionsService;
        }

        public async override Task<ContinueWith> Execute(CommandContext context)
        {
            var chatId = GetUserId(context);
            var userSubs = await _subs.GetActiveSubs(chatId);

            if (userSubs.Length == 0)
            {
                await SendMessage("Нет подписок", context);

                return null;
            }

            var subsNames = userSubs
                .Select(s => s.SearchQuery)
                .ToArray();
            await SendMessageWithButtons("Какой удалить?", context, TelegramMarkupHelper.MessageColumnBtns(subsNames));

            return ContinueWith.Command<ConfirmSubDelCommand>();
        }
    }
}
