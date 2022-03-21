using System;
using Telegram.Bot.Types;
using TelegramBot.UI.Bot.Context;

namespace TelegramBot.UI.Exceptions
{
	public class UpdateHandlerException : Exception
	{
		public UpdateHandlerException(Update update, BotContext context)
			:base($"No handler for update: [{update.Type}] context: [{context.State}]")
		{ }
	}
}

