using BikeScanner.UI.Bot.BotService.CommandsHandler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.UI.Bot.TelegramPoll
{
    internal class TelegramHostedService : IHostedService
    {
        private readonly ITelegramBotClient     _botClient;
        private readonly CommandsHandler        _commandsHandler;
        private readonly ILogger                _logger;
        private CancellationTokenSource         _cts;

        public TelegramHostedService(
            ITelegramBotClient telegramBotClient,
            CommandsHandler commandsHandler,
            ILogger<TelegramHostedService> logger)
        {
            _botClient = telegramBotClient;
            _commandsHandler = commandsHandler;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();
            _botClient.StartReceiving(
                HandleUpdateAsync, 
                HandleErrorAsync, 
                null,
                _cts.Token);

            _logger.LogInformation($"Start polling telegram bot.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation($"Stop polling telegram bot.");

            return Task.CompletedTask;
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            return _commandsHandler.Handle(update, botClient);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            return Task.CompletedTask;
        }
    }

}
