using System;

namespace BikeScanner.Domain.Models
{
    public class ContentEntity : BaseEntity
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public long IndexingStamp { get; set; }
        public string SourceType { get; set; }
    }
}
