using System;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.UI.Bot.Filters
{
	/// <summary>
    /// Update filters
    /// </summary>
	public static class FilterDefinitions
	{
		/// <summary>
        /// Filter by command name
        /// </summary>
        /// <param name="name">Command name</param>
        /// <exception cref="ArgumentException"></exception>
		public static CommandFilter Command(string name)
		{
			if (name.StartsWith("/"))
				throw new ArgumentException("Command name start with '/' error.");
			var commandName = $"/{name}";

			return (update, context) =>
				update.Type == UpdateType.Message &&
				update.Message.Type == MessageType.Text &&
				update.Message.Text.StartsWith(commandName);
		}

		public static CommandFilter Message(BotState state)
        {
			return (update, context) =>
				update.Type == UpdateType.Message &&
				update.Message.Type == MessageType.Text &&
				context.State == state;
        }

		public static CommandFilter CallbackCommand(string name)
        {
			return (update, context) =>
				update.Type == UpdateType.CallbackQuery &&
				update.CallbackQuery.Data.StartsWith(name);
        }

		/// <summary>
        /// User join filter
        /// </summary>
		public static CommandFilter JoinBot =>
			(update, context) =>
			update.Type == UpdateType.MyChatMember &&
			update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Member;

		/// <summary>
        /// User left filter
        /// </summary>
		public static CommandFilter LeftBot =>
			(update, context) =>
				update.Type == UpdateType.MyChatMember &&
				update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Kicked;
	}
}

