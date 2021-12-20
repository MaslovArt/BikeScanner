using BikeScanner.UI.Bot.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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
        public abstract string CancelWith { get; }

        public abstract Task<string> Execute(
            Update update, 
            ITelegramBotClient client);   
        
        protected string GetChatInput(Update update)
        {
            return update.Message?.Text ?? update.CallbackQuery?.Data;
        }

        protected long GetChatId(Update update)
        {
            return (update.Message?.Chat.Id ?? update.CallbackQuery.Message?.Chat.Id).Value;
        }

        protected Task SendMessage(
            string text, 
            Update update, 
            ITelegramBotClient telegramBotClient)
        {
            var chatId = GetChatId(update);
            return telegramBotClient.SendTextMessageAsync(chatId, text);
        }

        protected Task SendErrorMessage(
            Update update,
            ITelegramBotClient telegramBotClient)
        {
            var chatId = GetChatId(update);
            return telegramBotClient.SendTextMessageAsync(chatId, "Упс.\nЧто-то пошло не так.");
        }

        protected Task SendMessageKeyboard(
            string text,
            Update update, 
            ITelegramBotClient telegramBotClient, 
            params string[] options)
        {
            var chatId = GetChatId(update);
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

            return telegramBotClient.SendTextMessageAsync(chatId, text, replyMarkup: buttons);
        }

        protected Task SendMessageRowButtons(
            string text,
            Update update,
            ITelegramBotClient telegramBotClient,
            IEnumerable<string> options)
        {
            var chatId = GetChatId(update);

            var buttons = options
                .ToMax64BytesValue()
                .Select(o => InlineKeyboardButton.WithCallbackData(o));
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(buttons);

            return telegramBotClient.SendTextMessageAsync(chatId, text, replyMarkup: markup);
        }

        protected Task SendMessageColumnButtons(
            string text,
            Update update,
            ITelegramBotClient telegramBotClient,
            IEnumerable<string> options)
        {
            var chatId = GetChatId(update);

            var buttons = options
                .ToMax64BytesValue()
                .Select(o => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(o)
                });
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(buttons);

            return telegramBotClient.SendTextMessageAsync(chatId, text, replyMarkup: markup);
        }
    }
}
