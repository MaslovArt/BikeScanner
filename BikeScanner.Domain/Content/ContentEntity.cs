using BikeScanner.Domain.Base;
using System;

namespace BikeScanner.Domain.Content
{
    public class ContentEntity : BaseEntity
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public DateTime IndexTime { get; set; }
        public string SourceType { get; set; }
    }
}
