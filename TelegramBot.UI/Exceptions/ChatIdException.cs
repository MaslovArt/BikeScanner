using System;
using Telegram.Bot.Types;

namespace TelegramBot.UI.Exceptions
{
	public class ChatIdException : Exception
	{
		public ChatIdException(Update update)
			: base($"Can't get chat id for update {update.Type}")
		{ }
	}
}

