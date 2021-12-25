using System.Text.Json;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public class ContinueWith
    {
        public string Key { get; private set; }
        public string State { get; private set; }

        private ContinueWith()
        { }

        public static ContinueWith Command(string commandKey, object state = null)
        {
            return new ContinueWith()
            {
                Key = commandKey,
                State = state != null
                    ? JsonSerializer.Serialize(state)
                    : string.Empty
            };
        }

        public static ContinueWith Command<T>(object state = null)
        {
            return Command(typeof(T).Name, state);
        }
    }
}
