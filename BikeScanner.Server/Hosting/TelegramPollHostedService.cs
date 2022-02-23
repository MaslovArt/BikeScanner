using BikeScanner.Server.Controllers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.Server.Hosting
{
    /// <summary>
    /// Redirect polling telegram request to webhook controller (For development purpose).
    /// </summary>
    internal class TelegramPollHostedService : IHostedService
    {
        private readonly IServiceProvider       _serviceProvider;
        private readonly ITelegramBotClient     _botClient;
        private readonly ILogger        _logger;
        private CancellationTokenSource _cts;

        public TelegramPollHostedService(
            IServiceProvider serviceProvider,
            ITelegramBotClient telegramBotClient,
            ILogger<TelegramPollHostedService> logger
            )
        {
            _serviceProvider = serviceProvider;
            _botClient = telegramBotClient;
            _logger = logger;
            _cts = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                null,
                _cts.Token);
            _logger.LogInformation($"{nameof(TelegramPollHostedService)}: Start redirect polling to webhook.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation($"{nameof(TelegramPollHostedService)}: Stop.");

            return Task.CompletedTask;
        }

        public Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
            )
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider
                .GetRequiredService<TelegramUIController>()
                .Update(update);
        }

        private Task HandleErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken cancellationToken
            )
        {
            _logger.LogError(exception, exception.Message);

            return Task.CompletedTask;
        }
    }
}

