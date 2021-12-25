namespace BikeScanner.Domain.Models
{
    public class PagedEntities<T>
    {
        public T[] Entities { get; set; }
        public int Total { get; set; }
    }
}
