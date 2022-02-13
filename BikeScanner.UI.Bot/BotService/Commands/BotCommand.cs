using BikeScanner.UI.Bot.Extentions;
using System.Collections.Generic;
using System.Linq;
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

        protected Task SendErrorMessage(CommandContext context)
        {
            var chatId = GetChatId(context);
            return context.Client.SendTextMessageAsync(chatId, "Упс.\nЧто-то пошло не так.");
        }

        protected Task SendMessageKeyboard(
            string text,
            CommandContext context, 
            params string[] options)
        {
            var chatId = GetChatId(context);
            ReplyKeyboardMarkup buttons = null;
            if (options?.Length > 0)
            {
                var btns = new List<List<KeyboardButton>>
                {
                    options.Select(o => new KeyboardButton(o)).ToList()
                };
                buttons = new ReplyKeyboardMarkup(btns)
                {
                    OneTimeKeyboard = true
                };
            }

            return context.Client.SendTextMessageAsync(chatId, text, replyMarkup: buttons);
        }

        protected Task SendMessageRowButtons(
            string text,
            CommandContext context,
            IEnumerable<string> options)
        {
            var chatId = GetChatId(context);

            var buttons = options
                .ToMax64BytesValue()
                .Select(o => InlineKeyboardButton.WithCallbackData(o));
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(buttons);

            return context.Client.SendTextMessageAsync(chatId, text, replyMarkup: markup);
        }

        protected Task SendMessageColumnButtons(
            string text,
            CommandContext context,
            IEnumerable<string> options)
        {
            var chatId = GetChatId(context);

            var buttons = options
                .ToMax64BytesValue()
                .Select(o => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(o)
                });
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(buttons);

            return context.Client.SendTextMessageAsync(chatId, text, replyMarkup: markup);
        }
    }
}
