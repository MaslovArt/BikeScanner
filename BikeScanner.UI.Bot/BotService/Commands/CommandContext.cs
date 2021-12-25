using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public class CommandContext
    {
        private string _state;
        public Update Update { get; private set; }
        public ITelegramBotClient Client { get; private set; }

        public CommandContext(Update update, ITelegramBotClient client, string state)
        {
            Update = update;
            Client = client;
            _state = state;
        }

        public T GetState<T>()
        {
            return string.IsNullOrEmpty(_state)
                ? default(T)
                : JsonSerializer.Deserialize<T>(_state);
        }
    }
}
