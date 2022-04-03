namespace BikeScanner.Domain.Models
{
    public class Page<T>
    {
        /// <summary>
        /// Page entities
        /// </summary>
        public T[] Items { get; set; }
        /// <summary>
        /// Total entities count
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Next page offset
        /// </summary>
        public int Offset { get; set; }
    }
}
