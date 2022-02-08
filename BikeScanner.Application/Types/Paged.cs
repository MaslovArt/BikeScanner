namespace BikeScanner.Application.Types
{
    public class Paged<T>
    {
        /// <summary>
        /// Page items
        /// </summary>
        public T[] Items { get; set; }
        /// <summary>
        /// Total items count
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Next page offset
        /// </summary>
        public int Offset { get; set; }
    }
}
