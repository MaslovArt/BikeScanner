using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands.Main
{
    public class UnknownCommand : CommandBase
    {
        public override CommandFilter Filter => (_, _) => true;

        public override Task Execute(CommandContext context)
        {
            var wtfMessage = @$"Хм..
Я не знаю что с этим делать(
Тут мои возможности (/{CommandNames.UI.Help})";
            return SendMessage(wtfMessage, context);
        }
    }
}

