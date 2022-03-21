using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;
using BikeScanner.Domain.Models;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Main
{
    public class StartCommand : CommandBase
	{
        private readonly IUsersService _usersService;

        public StartCommand(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.Command(CommandNames.UI.Start);

        public async override Task Execute(CommandContext context)
        {
            var helloAgainMsg = string.Empty;
            var user = await _usersService.EnsureUser(UserId(context));
            if (user.AccountStatus == AccountStatus.Inactive)
            {
                helloAgainMsg = "С возвращением!";
                await _usersService.ActivateUser(user.Id);
            }

            var message = $"Привет! {helloAgainMsg} Я бот для поиска по объявлениям.";
            await SendMessage(message, context);
        }
    }
}

