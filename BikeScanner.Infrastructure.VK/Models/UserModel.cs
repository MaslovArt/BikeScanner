using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class UserModel : VkUnitModel
    {
        public int UnitId => Id;

        public string UnitName => $"{FirstName} {LastName}";

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("photo_50")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("is_closed")]
        public bool IsClosed { get; set; }
    }
}
