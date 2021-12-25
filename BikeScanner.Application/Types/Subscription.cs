namespace BikeScanner.Application.Types
{
    public class Subscription
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string SubscriptionType { get; set; }
        public string SearchQuery { get; set; }
    }
}
