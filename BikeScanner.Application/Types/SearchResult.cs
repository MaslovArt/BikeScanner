namespace BikeScanner.Application.Types
{
    public class SearchResult
    {
        public SearchResult(string query, string adUrl)
        {
            Query = query;
            AdUrl = adUrl;
        }

        public string Query { get; set; }
        public string AdUrl { get; set; }
    }
}
