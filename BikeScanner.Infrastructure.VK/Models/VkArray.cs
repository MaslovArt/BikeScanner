using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class VkArray<T>
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("items")]
        public T[] Items { get; set; }
    }
}
