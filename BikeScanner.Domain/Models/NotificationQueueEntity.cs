using System;

namespace BikeScanner.Domain.Models
{
    public class NotificationQueueEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string AdUrl { get; set; }
        public string SearchQuery { get; set; }
        public string NotificationType { get; set; }
        public DateTime? SendTime { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
