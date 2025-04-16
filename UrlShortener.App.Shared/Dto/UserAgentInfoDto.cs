using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents structured information about a user's environment, parsed from a user agent string.
    /// </summary>
    public class UserAgentInfoDto
    {
        /// <summary>
        /// Gets or sets the browser family name (e.g., Chrome, Firefox, Safari).
        /// </summary>
        [JsonPropertyName("browser_family")]
        public string BrowserFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets detailed client information such as browser name, type, and engine.
        /// </summary>
        [JsonPropertyName("client")]
        public UserAgentClientDto? Client { get; set; }

        /// <summary>
        /// Gets or sets information about the user's device, including brand, model, and type.
        /// </summary>
        [JsonPropertyName("device")]
        public UserAgentDeviceDto? Device { get; set; }

        /// <summary>
        /// Gets or sets the operating system information such as name and version.
        /// </summary>
        [JsonPropertyName("os")]
        public UserAgentOsDto? Os { get; set; }

        /// <summary>
        /// Gets or sets the operating system family (e.g., Windows, macOS, Android).
        /// </summary>
        [JsonPropertyName("os_family")]
        public string OsFamily { get; set; } = string.Empty;
    }
}
