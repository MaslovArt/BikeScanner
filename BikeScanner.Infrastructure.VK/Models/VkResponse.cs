using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class VkResponse<T>
    {
        [JsonIgnore]
        public bool Succeed => Error == null;

        [JsonPropertyName("response")]
        public T Response { get; set; }

        [JsonIgnore]
        public VkError Error { get; set; }
    }
}
