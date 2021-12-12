namespace BikeScanner.Domain.Models
{
    public class NotificationQueueEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string ContentUrl { get; set; }
        public string SearchQuery { get; set; }
        public string Type { get; set; }
    }
}
