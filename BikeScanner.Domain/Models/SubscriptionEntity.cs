using System;

namespace BikeScanner.Domain.Models
{
    public class SubscriptionEntity : BaseEntity
    {
        public long UserId { get; set; }
        public DateTime Created { get; set; }
        public string SearchQuery { get; set; }
    }
}
