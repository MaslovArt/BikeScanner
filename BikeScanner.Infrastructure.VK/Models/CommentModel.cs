using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.VK.Models
{
    public class CommentModel
    {
        [JsonPropertyName("from_id")]
        public int AuthorId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
