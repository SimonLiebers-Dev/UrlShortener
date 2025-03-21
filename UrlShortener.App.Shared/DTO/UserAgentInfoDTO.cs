using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.DTO
{
    public class UserAgentInfoDTO
    {
        [JsonPropertyName("browser_family")]
        public string BrowserFamily { get; set; } = string.Empty;

        [JsonPropertyName("client")]
        public UserAgentClientDTO? Client { get; set; }

        [JsonPropertyName("device")]
        public UserAgentDeviceDTO? Device { get; set; }

        [JsonPropertyName("os")]
        public UserAgentOsDTO? Os { get; set; }

        [JsonPropertyName("os_family")]
        public string OsFamily { get; set; } = string.Empty;
    }
}
