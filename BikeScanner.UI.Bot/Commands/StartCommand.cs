using BikeScanner.Application.Services.UsersService;
using BikeScanner.Domain.Models;
using BikeScanner.UI.Bot.BotService.Commands;
using System.Threading.Tasks;

namespace BikeScanner.UI.Bot.Commands
{
    /// <summary>
    /// Init bot
    /// </summary>
    public class StartCommand : BotUICommand, IStartBotCommand
    {
        private readonly IUsersService _usersService;

        public override string CallName => UICommands.Start;
        public override string Description => "Запуск бота";

        public StartCommand(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override async Task<ContinueWith> Execute(CommandContext context)
        {
            var userId = GetUserId(context);

            var currentUser = await _usersService.EnsureUser(userId);
            if (currentUser.AccountStatus == AccountStatus.Inactive)
                await _usersService.ActivateUser(userId);

            var message = @$"Привет!
Я бот для поиска по объявлениям. 
Все любят дешманские бу и не очень мтб детальки. Я увижу их первыми и сразу же сообщу.

Посмотреть возможности - ({UICommands.Help}).";
            await SendMessage(message, context);

            return null;
        }
    }
}
