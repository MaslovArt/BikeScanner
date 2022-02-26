using System;
using System.Linq;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    /// <summary>
    /// Base class for callable from chat commands.
    /// </summary>
    public abstract class BotUICommand : BotCommand, IBotUICommand
    {
        public abstract string CallName { get; }
        public abstract string Description { get; }
        public override bool ExecuteImmediately => true;
        public override CancelWith CancelWith => null;

        public bool CanHandel(string command)
        {
            return !string.IsNullOrWhiteSpace(command) && command.StartsWith($"{CallName}");
        }

        /// <summary>
        /// Get param value or null if not exists
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paramInd"></param>
        /// <returns>String value or null</returns>
        protected string GetParam(CommandContext context, int paramInd)
        {
            var commandParams = GetParams(context);
            return commandParams.Length > paramInd
                ? commandParams[paramInd]
                : null;
        }

        /// <summary>
        /// Get params values
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string[] GetParams(CommandContext context)
        {
            var text = GetChatInput(context);
            return text
                .Replace(CallName, "")
                .Trim()
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }
    }
}
