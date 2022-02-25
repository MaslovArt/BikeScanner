using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    /// <summary>
    /// Base class for bot commands.
    /// </summary>
    public abstract class BotCommand : IBotCommand
    {
        public string Key => GetType().Name;
        public abstract bool ExecuteImmediately { get; }
        public abstract CancelWith CancelWith { get; }

        public abstract Task<ContinueWith> Execute(CommandContext context);   
        
        protected string GetChatInput(CommandContext context)
        {
            var input = context.Update.Message?.Text ?? context.Update.CallbackQuery?.Data;

            return input.Trim();
        }

        protected long GetChatId(CommandContext context)
        {
            return (context.Update.Message?.Chat.Id ?? context.Update.CallbackQuery.Message?.Chat.Id).Value;
        }

        protected Task SendMessage(string text, CommandContext context)
        {
            var chatId = GetChatId(context);
            return context.Client.SendTextMessageAsync(chatId, text);
        }

        protected Task SendMessageWithButtons(string text, CommandContext context, IReplyMarkup markup)
        {
            var chatId = GetChatId(context);
            return context.Client.SendTextMessageAsync(chatId, text, replyMarkup: markup);
        }
    }
}
