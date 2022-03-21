using System;
using System.Threading.Tasks;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Search
{
    public class SearchCommand : CommandBase
    {
        public override CommandFilter Filter =>
            FilterDefinitions.Command(CommandNames.UI.Search);

        public async override Task Execute(CommandContext context)
        {
            await SendMessage("Что найти?", context);
            context.BotContext.State = BotState.WaitSearchInput;
        }
    }
}

