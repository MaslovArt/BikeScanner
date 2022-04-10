using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Services.Vk.Models
{
    internal class VkErrorResponse
    {
        [JsonPropertyName("error")]
        public VkError Error { get; set; }
    }
}
