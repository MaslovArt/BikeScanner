using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class GroupModel : VkUnitModel
    {
        public int UnitId => -1 * Id;

        public string UnitName => Name;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("screen_name")]
        public string ScreeName { get; set; }

        [JsonPropertyName("photo_50")]
        public string AvatarUrl { get; set; }
    }
}
