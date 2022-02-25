using BikeScanner.UI.Bot.BotService.Commands;
using BikeScanner.UI.Bot.Helpers;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Init bot
    /// </summary>
    public class StartCommand : BotUICommand, IStartBotCommand
    {
        public override string CallName => UICommands.Start;
        public override string Description => "Запуск бота";

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var message = @$"Привет!
Я бот для поиска по объявлениям. 
Все любят дешманские бу и не очень мтб детальки. Я увижу их первыми и сразу же сообщу.

Посмотреть возможности - ({UICommands.Help}).";

            var mainButtons = new string[]
            {
                UICommands.Search,
                UICommands.Cancel
            };
            var markup = TelegramMarkupHelper.KeyboardRowBtns(mainButtons);
            await SendMessageWithButtons(message, context, markup);

            return null;
        }
    }
}
