using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.DTO
{
    public class UserAgentDeviceDTO
    {
        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
