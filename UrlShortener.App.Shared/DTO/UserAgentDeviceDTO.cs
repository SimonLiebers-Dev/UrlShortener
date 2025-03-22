using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class UserAgentDeviceDto
    {
        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
