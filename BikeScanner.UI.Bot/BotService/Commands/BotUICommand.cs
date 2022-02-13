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
            return !string.IsNullOrWhiteSpace(command) && command.Contains($"{CallName}");
        }

        protected string GetParam(CommandContext context, int paramInd)
        {
            return GetParams(context)[paramInd];
        }

        protected string[] GetParams(CommandContext context)
        {
            var text = GetChatInput(context);
            return text
                .Replace(CallName, "")
                .Trim()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }
    }
}
