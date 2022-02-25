using System.Threading.Tasks;
using BikeScanner.Application.Interfaces;
using Telegram.Bot;

namespace BikeScanner.Infrastructure.Notificators
{

    public class TelegramNotificator : INotificator
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramNotificator(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public Task Send(long userId, string message)
        {
            return _telegramBotClient.SendTextMessageAsync(userId, message);
        }
    }
}

