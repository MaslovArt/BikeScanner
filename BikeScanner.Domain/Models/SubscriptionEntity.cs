namespace BikeScanner.Domain.Models
{
    public class SubscriptionEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string NotificationType { get; set; }
        public string SearchQuery { get; set; }
        public SubscriptionStatus Status { get; set; }
    }
}
