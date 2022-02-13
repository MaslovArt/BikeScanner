using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    /// <summary>
    /// Interface for bot commands.
    /// </summary>
    public interface IBotCommand
    {
        /// <summary>
        /// Command key
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Execute immediately or delay until next user message
        /// </summary>
        bool ExecuteImmediately { get; }
        /// <summary>
        /// Command (Key) that executes on cancelation
        /// </summary>
        CancelWith CancelWith { get; }
        /// <summary>
        /// Execute current command
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="client"></param>
        /// <returns>Next command key</returns>
        Task<ContinueWith> Execute(CommandContext context);
        /// <summary>
        /// Can command handle message
        /// </summary>
        /// <param name="command">User message</param>
        /// <returns></returns>
    }
}
