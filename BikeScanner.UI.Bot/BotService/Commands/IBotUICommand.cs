namespace BikeScanner.UI.Bot.BotService.Commands
{
    /// <summary>
    /// Interface for callable from chat commands.
    /// </summary>
    public interface IBotUICommand : IBotCommand
    {
        /// <summary>
        /// Chat message that runs this command
        /// </summary>
        string CallName { get; }

        /// <summary>
        /// Command description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Can command handle current request
        /// </summary>
        /// <param name="command">Current command request</param>
        /// <returns></returns>
        bool CanHandel(string command);
    }
}
