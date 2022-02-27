using BikeScanner.UI.Bot.BotService.Config;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BikeScanner.Server.Hosting
{
    public class ConfigureWebhookHostedService : IHostedService
    {
        private readonly ILogger<ConfigureWebhookHostedService> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly TelegramApiAccessConfig _botConfig;

        public ConfigureWebhookHostedService(
            ILogger<ConfigureWebhookHostedService> logger,
            ITelegramBotClient telegramBotClient,
            IOptions<TelegramApiAccessConfig> options)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _botConfig = options.Value;

            if (string.IsNullOrEmpty(_botConfig.Webhook))
                throw new ArgumentException("No webhook configuration!");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookAddress = @$"{_botConfig.Webhook}/bot/{_botConfig.Key}";
            _logger.LogInformation("Setting webhook: {webhookAddress}", webhookAddress);
            await _telegramBotClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing webhook");
            await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
