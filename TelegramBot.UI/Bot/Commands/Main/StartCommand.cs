using System.Threading.Tasks;
using BikeScanner.Application.Services.UsersService;
using TelegramBot.UI.Bot.Helpers;

namespace TelegramBot.UI.Bot.Commands.Main
{
    /// <summary>
    /// Start bot
    /// </summary>
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
            var userId = UserId(context);
            var helloAgainMsg = string.Empty;
            var user = await _usersService.EnsureActiveUser(userId);

            var message = @$"Привет {user.Login}! {helloAgainMsg} Я бот для поиска по объявлениям.
Для поиска нужно запустить комманду ({CommandNames.UI.Search}), дальше думаю разберетесь)
Если желаемого найти не удалось, можно добавить подписку на поиск ({CommandNames.UI.AddSub}).
Как только появится похожее объявление, я сообщу.
Подписка больше неактуальна - удаляй ее ({CommandNames.UI.DeleteSub}).

Хочешь сообщить об ошибке в работе бота, предложить улучшения или сказать что это полная хрень и послать автора - {CommandNames.UI.DevMessage}.

Список доступных команд:
{CommandNames.UI.Search} - Поиск
{CommandNames.UI.MySubs} - Мои подписки
{CommandNames.UI.AddSub} - Добавить подписку
{CommandNames.UI.DeleteSub} - Удалить подписку
{CommandNames.UI.DevMessage} - Сообщение админу
{CommandNames.UI.Start} - Перезапуск бота";
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

