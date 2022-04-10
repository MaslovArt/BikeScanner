using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Services.Vk.Models
{
    internal class VkResponse<T>
    {
        [JsonPropertyName("response")]
        public T Response { get; set; }
    }
}
