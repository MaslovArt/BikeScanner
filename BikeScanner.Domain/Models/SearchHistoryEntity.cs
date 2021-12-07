using System;

namespace BikeScanner.Domain.Models
{
    public class SearchHistoryEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string SearchQuery { get; set; }
        public DateTime Date { get; set; }
    }
}
