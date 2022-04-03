using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.UI.Exceptions;

namespace TelegramBot.UI.Bot.Commands
{
    public abstract class CommandBase : ICommandBase
    {
        public abstract Task Execute(CommandContext context);

        public abstract CommandFilter Filter { get; }

        /// <summary>
        /// Base params separator symbol
        /// </summary>
        protected char ParamSeparator => ';';

        /// <summary>
        /// Get input as param
        /// </summary>
        /// <param name="context"></param>
        /// <param name="param">Param index</param>
        /// <param name="exclude">Exclude command name from input</param>
        /// <returns></returns>
        protected string GetParam(
            CommandContext context,
            int param,
            string exclude = null
            )
        {
            var input = ChatInput(context);
            var paramsInput = exclude == null
                ? input
                : input.Replace(exclude, string.Empty).Trim();

            return paramsInput.Split(ParamSeparator)[param];
        }

        /// <summary>
        /// User input from message or callback
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Message or callback text or empty string</returns>
        protected string ChatInput(CommandContext context)
        {
            var input = context.Update.Message?.Text
                ?? context.Update.CallbackQuery?.Data
                ?? string.Empty;

            return input.Trim();
        }

        /// <summary>
        /// Extract user id from message, callback or myChatMemeber
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ChatIdException"></exception>
        protected long UserId(CommandContext context)
        {
            return
                context.Update.Message?.Chat.Id ??
                context.Update.CallbackQuery?.Message?.Chat.Id ??
                context.Update.MyChatMember?.Chat.Id ??
                throw new ChatIdException(context.Update);
        }

        protected Task AnswerCallback(string text, CommandContext context)
        {
            var callbackId = context.Update.CallbackQuery.Id;
            return context.Client.AnswerCallbackQueryAsync(callbackId, text, false);
        }

        protected Task SendMessages(IEnumerable<string> messages, CommandContext context)
        {
            var tasks = messages.Select(m => SendMessage(m, context));
            return Task.WhenAll(tasks);
        }

        protected Task SendMessage(string message, CommandContext context)
        {
            var chatId = UserId(context);
            return context.Client.SendTextMessageAsync(chatId, message);
        }

        protected Task SendMessageWithButtons(
            string message,
            CommandContext context,
            IReplyMarkup markup
            )
        {
            var chatId = UserId(context);
            return context.Client.SendTextMessageAsync(chatId, message, replyMarkup: markup);
        }
    }
}

