using System.Threading.Tasks;

namespace TelegramBot.UI.Bot.Commands
{
	public abstract class CommandUIBase : CommandBase
	{
        public abstract Task ExecuteCommand(CommandContext context);

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.Default;

            return ExecuteCommand(context);
        }
    }
}

