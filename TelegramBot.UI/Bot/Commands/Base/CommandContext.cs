using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.UI.Bot.Context;

namespace TelegramBot.UI.Bot.Commands
{
	public class CommandContext
	{
        public BotContext BotContext { get; private set; }
        public Update Update { get; private set; }
        public ITelegramBotClient Client { get; private set; }

        public CommandContext(Update update, ITelegramBotClient client, BotContext botContext)
        {
            Update = update;
            Client = client;
            BotContext = botContext;
        }
    }
}

