using System.Threading.Tasks;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands
{
    /// <summary>
    /// Telegram update handler
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// Execute command handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task Execute(CommandContext context);

        /// <summary>
        /// Update handler filter
        /// </summary>
        CommandFilter Filter { get; }
    }
}