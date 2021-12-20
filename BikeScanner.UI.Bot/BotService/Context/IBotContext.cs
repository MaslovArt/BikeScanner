using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Context
{
    public interface IBotContext
    {
        Task<BotContextModel> GetUserContext(long userId);
        Task<BotContextModel> EnsureContext(long userId);
        Task Update(BotContextModel context);
    }
}
