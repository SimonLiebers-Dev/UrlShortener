using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class UserStatsDto
    {
        [JsonPropertyName("clicks")]
        public int Clicks { get; set; } = 0;

        [JsonPropertyName("mappings")]
        public int Mappings { get; set; } = 0;
    }
}
