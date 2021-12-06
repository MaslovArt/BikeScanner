using BikeScanner.Domain.Base;

namespace BikeScanner.Domain.Subscriptions
{
    public class SubscriptionEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string SearchQuery { get; set; }
    }
}
