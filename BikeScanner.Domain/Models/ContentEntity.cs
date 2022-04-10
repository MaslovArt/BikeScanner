using System;

namespace BikeScanner.Domain.Models
{
    public class ContentEntity : BaseEntity
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public long IndexEpoch { get; set; }
        public string SourceType { get; set; }
    }
}
