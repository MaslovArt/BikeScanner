namespace BikeScanner.Application.Types
{
    public class Paged<T>
    {
        public T[] Items { get; set; }
        public int Total { get; set; }
    }
}
