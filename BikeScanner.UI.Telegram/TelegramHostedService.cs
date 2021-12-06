using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace BikeScanner.Telegram
{
    internal class TelegramHostedService : IHostedService
    {
        //private readonly ITelegramBotClient     _botClient;
        //private readonly IBotService            _botService;
        //private readonly TelegramBotSettings    _botConfig;
        private readonly ILogger                _logger;
        private CancellationTokenSource         _cts;

        public TelegramHostedService(
            //ITelegramBotClient telegramBotClient,
            //IBotService botService,
            //IOptions<TelegramBotSettings> botConfig,
            ILogger<TelegramHostedService> logger)
        {
            //_botClient = telegramBotClient;
            //_botService = botService;
            //_botConfig = botConfig.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();
            //_botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), _cts.Token);

            //_logger.LogInformation($"Start polling telegram bot {_botConfig.BotName}.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            //_logger.LogInformation($"Stop polling telegram bot {_botConfig.BotName}.");

            return Task.CompletedTask;
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //return _botService.Update(update, botClient);
            return Task.FromResult(1);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            return Task.CompletedTask;
        }
    }

}
