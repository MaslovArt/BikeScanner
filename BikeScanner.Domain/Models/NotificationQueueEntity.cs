using System;

namespace BikeScanner.Domain.Models
{
    public class NotificationQueueEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string Text { get; set; }
        public DateTime? SendTime { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
