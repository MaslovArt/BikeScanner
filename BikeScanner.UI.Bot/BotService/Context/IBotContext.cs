using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.BotService.Context
{
    public interface IBotContext
    {
        Task<BotContext> GetUserContext(long userId);
        Task<BotContext> EnsureContext(long userId);
        Task Update(BotContext context);
    }
}
