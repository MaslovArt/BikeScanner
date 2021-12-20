using System;
using System.Linq;
using Telegram.Bot.Types;

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

        public bool CanHandel(string command)
        {
            return !string.IsNullOrWhiteSpace(command) && command.Contains($"{CallName}");
        }

        protected string GetParam(Update update, int paramInd)
        {
            return GetParams(update)[paramInd];
        }

        protected string[] GetParams(Update update)
        {
            var text = GetChatInput(update);
            return text
                .Replace(CallName, "")
                .Trim()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }
    }
}
