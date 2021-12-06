using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class VkErrorResponse
    {
        [JsonPropertyName("error")]
        public VkError Error { get; set; }
    }
}
