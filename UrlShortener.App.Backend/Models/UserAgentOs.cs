using System.Text.Json.Serialization;

namespace UrlShortener.App.Backend.Models
{
    internal class UserAgentOs
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}
