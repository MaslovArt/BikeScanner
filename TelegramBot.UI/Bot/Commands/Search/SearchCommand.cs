using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.Search
{
    public class SearchCommand : CommandBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.Search),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.Search));

        public async override Task Execute(CommandContext context)
        {
            await SendMessage("Что найти?", context);
            context.BotContext.State = BotState.WaitSearchInput;
        }
    }
}

