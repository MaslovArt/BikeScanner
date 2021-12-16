namespace BikeScanner.Application.Models
{
    public class SearchResultModel
    {
        public SearchResultModel(string query, string adUrl)
        {
            Query = query;
            AdUrl = adUrl;
        }

        public string Query { get; set; }
        public string AdUrl { get; set; }
    }
}
