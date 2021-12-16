using BikeScanner.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BikeScanner.Infrastructure.Notificators
{
    public class TestLoggerNotificator : INotificator
    {
        private readonly ILogger<TestLoggerNotificator> _logger;

        public TestLoggerNotificator(ILogger<TestLoggerNotificator> logger)
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
