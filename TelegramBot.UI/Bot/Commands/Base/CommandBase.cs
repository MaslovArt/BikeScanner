using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.UI.Bot.Filters;
using TelegramBot.UI.Exceptions;

namespace TelegramBot.UI.Bot.Commands
{
    public abstract class CommandBase : ICommandBase
    {
        public abstract Task Execute(CommandContext context);

        public abstract CommandFilter Filter { get; }

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

            return paramsInput.Split(';')[param];
        }

        protected string ChatInput(CommandContext context)
        {
            var input = context.Update.Message?.Text
                ?? context.Update.CallbackQuery?.Data
                ?? string.Empty;

            return input.Trim();
        }

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
            return context.Client.AnswerCallbackQueryAsync(callbackId, text, true);
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

