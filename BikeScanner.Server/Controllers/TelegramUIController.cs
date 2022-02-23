using BikeScanner.UI.Bot.BotService.CommandsHandler;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BikeScanner.Server.Controllers
{
    /// <summary>
    /// Handle request from telegram app
    /// </summary>
    public class TelegramUIController : BaseApiController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly CommandsHandler _commandsHandler;

        public TelegramUIController(
            ITelegramBotClient telegramBotClient,
            CommandsHandler commandsHandler
            )
        {
            _telegramBotClient = telegramBotClient;
            _commandsHandler = commandsHandler;
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            await _commandsHandler.Handle(update, _telegramBotClient);
            return Ok();
        }
    }
}

