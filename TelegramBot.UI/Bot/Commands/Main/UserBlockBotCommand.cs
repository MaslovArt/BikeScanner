using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Main
{
    public class UserBlockBotCommand : CommandBase
    {
        private readonly IUsersService _usersService;

        public UserBlockBotCommand(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override CommandFilter Filter => FilterDefinitions.LeftBot;

        public override Task Execute(CommandContext context) =>
            _usersService.DiactivateUser(UserId(context));
    }
}

