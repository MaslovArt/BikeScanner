using Telegram.Bot.Types;
using TelegramBot.UI.Bot.Context;

namespace TelegramBot.UI.Bot.Filters
{
    public delegate bool CommandFilter(Update update, BotContext context);
}

