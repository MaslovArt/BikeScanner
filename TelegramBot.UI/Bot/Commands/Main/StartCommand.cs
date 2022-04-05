using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;
using BikeScanner.Domain.Models;

namespace TelegramBot.UI.Bot.Commands.Main
{
    /// <summary>
    /// Start bot
    /// </summary>
    public class StartCommand : HelpCommand
	{
        private readonly IUsersService _usersService;

        public StartCommand(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.UICommand(CommandNames.UI.Start);

        public async override Task ExecuteCommand(CommandContext context)
        {
            var userId = UserId(context);
            var helloAgainMsg = string.Empty;
            var user = await _usersService.EnsureUser(userId);

            if (user.State == AccountState.Inactive)
            {
                await _usersService.ActivateUser(userId);
            }

            var message = @$"Привет!
{helloAgainMsg}
Я бот для поиска по объявлениям.

Список доступных команд:
{CommandNames.UI.Search} - Поиск
{CommandNames.UI.MySubs} - Мои подписки
{CommandNames.UI.AddSub} - Добавить подписку
{CommandNames.UI.DeleteSub} - Удалить подписку
{CommandNames.UI.DevMessage} - Сообщение админу
{CommandNames.UI.Start} - Перезапуск бота";
            await SendMessage(message, context);
            await base.ExecuteCommand(context);
        }
    }
}

