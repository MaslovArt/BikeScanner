namespace BikeScanner.Domain.Models
{
    public class SubscriptionEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string SearchQuery { get; set; }
    }
}
