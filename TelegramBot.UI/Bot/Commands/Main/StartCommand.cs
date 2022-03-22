using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;
using BikeScanner.Domain.Models;
using TelegramBot.UI.Bot.Helpers;

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
            FilterDefinitions.UICommand(CommandNames.UI.Start);

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
            var btns = new string[]
            {
                CommandNames.AlternativeUI.Search,
                CommandNames.AlternativeUI.MySubs,
                CommandNames.AlternativeUI.Help
            };
            var keyboard = TelegramMarkupHelper.KeyboardRowBtns(btns);
            await SendMessageWithButtons(message, context, keyboard);
        }
    }
}

