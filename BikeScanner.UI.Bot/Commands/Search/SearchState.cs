namespace BikeScanner.UI.Bot.Commands.Search
{
    internal record SearchState
    {
        public string SearchQuery { get; set; }
        public int Skip { get; set; }
    }
}
