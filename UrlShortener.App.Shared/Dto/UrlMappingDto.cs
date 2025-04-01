using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class UrlMappingDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("long_url")]
        public string LongUrl { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("short_url")]
        public string ShortUrl { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("redirect_logs")]
        public List<RedirectLogDto> RedirectLogs { get; set; } = [];
    }
}
