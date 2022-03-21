using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.UI.Bot.Helpers
{
	public static class TelegramButtonsHelper
	{
		public const string AcceptButtonValue = "Да";
		public const string CancelButtonValue = "Нет";
		public static IReplyMarkup BooleanButtons =>
			TelegramMarkupHelper.MessageRowBtns(new string[]
			{
				AcceptButtonValue,
				CancelButtonValue
			});
	}
}

