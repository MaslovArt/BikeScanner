using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Context
{
    public interface IBotContextService
	{
		Task<BotContext> GetUserContext(long userId);
		Task<BotContext> EnsureContext(long userId);
		Task Update(BotContext context);
	}
}
