using System;
namespace TelegramBot.UI.Bot.Context
{
	public class BotContext
	{
		public long UserId { get; set; }
		public BotState State { get; set; }
	}
}

