using BikeScanner.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BikeScanner.Infrastructure.Notificators
{
    public class LogNotificator : INotificator
    {
        private readonly ILogger<LogNotificator> _logger;

        public LogNotificator(ILogger<LogNotificator> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(string type) => true;

        public Task Send(long userId, string message)
        {
            _logger.LogInformation($"For [{userId}] => {message}");
            return Task.FromResult(0);
        }
    }
}
