using System;
namespace TelegramBot.UI.Bot.Commands
{
	public static class CommandNames
	{
        public static class UI
        {
            public const string Start = "start";
            public const string Help = "help";
            public const string Search = "search";
            public const string UserSubs = "my_subs";
            public const string DeleteSub = "delete_sub";
            public const string ScrollFeed = "feed";
        }
        public static class Internal
        {
            public const string SaveSearch = "save_search";
            public const string MoreSearchResults = "more_search_results";
            public const string ConfirmDeleteSub = "confirm_delete_sub";
            public const string ApplyDeleteSub = "apply_delete_sub";
        }
    }
}

