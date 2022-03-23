using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.Search
{
    /// <summary>
    /// Search. Ask what to search.
    /// </summary>
    public class SearchCommand : CommandBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.Search),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.Search));

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.WaitSearchInput;
            return SendMessage("Что найти?", context);
        }
    }
}

