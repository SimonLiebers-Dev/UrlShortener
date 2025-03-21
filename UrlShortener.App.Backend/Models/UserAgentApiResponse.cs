using System.Text.Json.Serialization;

namespace UrlShortener.App.Backend.Models
{
    public class UserAgentApiResponse
    {
        [JsonPropertyName("browser_family")]
        public string BrowserFamily { get; set; } = string.Empty;

        [JsonPropertyName("client")]
        public UserAgentClient? Client { get; set; }

        [JsonPropertyName("device")]
        public UserAgentDevice? Device { get; set; }

        [JsonPropertyName("os")]
        public UserAgentOs? Os { get; set; }

        [JsonPropertyName("os_family")]
        public string OsFamily { get; set; } = string.Empty;
    }
}
