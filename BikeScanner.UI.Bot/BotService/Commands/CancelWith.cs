using System.Text.Json;

namespace BikeScanner.UI.Bot.BotService.Commands
{
    public class CancelWith
    {
        public string Key { get; private set; }

        private CancelWith()
        { }

        public static CancelWith Command(string commandKey)
        {
            return new CancelWith()
            {
                Key = commandKey
            };
        }

        public static CancelWith Command<T>()
        {
            return Command(typeof(T).Name);
        }
    }
}
