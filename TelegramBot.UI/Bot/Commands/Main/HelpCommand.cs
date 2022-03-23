using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.Main
{
    /// <summary>
    /// Explain how to use the bot
    /// </summary>
    public class HelpCommand : CommandBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.Help),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.Help));

        public override Task Execute(CommandContext context)
        {
            var helpMessage = @$"
Для поиска по объявлением нужно запустить комманду ({CommandNames.UI.Search}), дальше думаю разберетесь)
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

            return SendMessage(helpMessage, context);
        }
    }
}

