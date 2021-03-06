using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;

namespace TelegramBot.UI.Bot.Commands.Main
{
    /// <summary>
    /// User stop bot
    /// </summary>
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

