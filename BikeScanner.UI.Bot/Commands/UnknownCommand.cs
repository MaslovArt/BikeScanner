using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotCommand = BikeScanner.UI.Bot.BotService.Commands.BotCommand;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Command for unclear situations
    /// </summary>
    public class UnknownCommand : BotCommand, IUnknownCommand
    {
        public override bool ExecuteImmediately => true;
        public override string CancelWith => null;

        public override async Task<string> Execute(Update update, ITelegramBotClient client)
        {
            var reply = $"И что мне с этим делать?\nМои возможности ({UICommands.Help}).";
            await SendMessage(reply, update, client);

            return null;
        }
    }
}
