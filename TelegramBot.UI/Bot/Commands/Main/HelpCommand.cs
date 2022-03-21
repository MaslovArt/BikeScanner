using System;
using System.Threading.Tasks;
using TelegramBot.UI.Bot.Filters;

namespace TelegramBot.UI.Bot.Commands.Main
{
    public class HelpCommand : CommandBase
    {
        public override CommandFilter Filter =>
            FilterDefinitions.Command(CommandNames.UI.Help);

        public override Task Execute(CommandContext context)
        {
            var helpMessage = @$"
/{CommandNames.UI.Start} - Запуск бота
";

            return SendMessage(helpMessage, context);
        }
    }
}

