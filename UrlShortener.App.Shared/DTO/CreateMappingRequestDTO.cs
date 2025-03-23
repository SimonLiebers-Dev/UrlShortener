using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class CreateMappingRequestDto
    {
        [JsonPropertyName("long_url")]
        public string LongUrl { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = string.Empty;
    }
}
