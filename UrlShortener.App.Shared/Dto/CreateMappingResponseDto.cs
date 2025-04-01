using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class CreateMappingResponseDto
    {
        [JsonPropertyName("short_url")]
        public string ShortUrl { get; set; } = string.Empty;
    }
}
