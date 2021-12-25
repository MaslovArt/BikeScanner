namespace BikeScanner.UI.Bot.Commands.Search
{
    internal class SearchState
    {
        public string SearchQuery { get; private set; }
        public int Skip { get; private set; }

        public SearchState(string searchQuery, int skip)
        {
            SearchQuery = searchQuery;
            Skip = skip;
        }
    }
}
