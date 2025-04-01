using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class UserAgentInfoDto
    {
        [JsonPropertyName("browser_family")]
        public string BrowserFamily { get; set; } = string.Empty;

        [JsonPropertyName("client")]
        public UserAgentClientDto? Client { get; set; }

        [JsonPropertyName("device")]
        public UserAgentDeviceDto? Device { get; set; }

        [JsonPropertyName("os")]
        public UserAgentOsDto? Os { get; set; }

        [JsonPropertyName("os_family")]
        public string OsFamily { get; set; } = string.Empty;
    }
}
