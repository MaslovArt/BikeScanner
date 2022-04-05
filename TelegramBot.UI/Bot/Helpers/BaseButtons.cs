using TelegramBot.UI.Bot.Commands;

namespace TelegramBot.UI.Bot.Helpers
{
	public static class BaseButtons
	{
		public static (string, string) Cancel =>
			($"{Emoji.ArrowLeft} Отмена", CommandNames.Internal.Cancel);
	}
}

