using BikeScanner.Domain.Base;

namespace BikeScanner.Domain.NotificationsQueue
{
    public class NotificationQueueEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string ContentUrl { get; set; }
        public string SearchQuery { get; set; }
    }
}
