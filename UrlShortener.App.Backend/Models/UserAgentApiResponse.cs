using System.Text.Json.Serialization;

namespace UrlShortener.App.Backend.Models
{
    /// <summary>
    /// Represents parsed user agent information returned by an external user agent API.
    /// </summary>
    public class UserAgentApiResponse
    {
        /// <summary>
        /// Gets or sets the browser family (e.g., Chrome, Firefox, Safari).
        /// </summary>
        [JsonPropertyName("browser_family")]
        public string BrowserFamily { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets detailed client information such as name, type, and engine.
        /// </summary>
        [JsonPropertyName("client")]
        public UserAgentClient? Client { get; set; }

        /// <summary>
        /// Gets or sets the device information such as brand, model, and type.
        /// </summary>
        [JsonPropertyName("device")]
        public UserAgentDevice? Device { get; set; }

        /// <summary>
        /// Gets or sets the operating system information including name and version.
        /// </summary>
        [JsonPropertyName("os")]
        public UserAgentOs? Os { get; set; }

        /// <summary>
        /// Gets or sets the operating system family (e.g., Windows, macOS, Linux).
        /// </summary>
        [JsonPropertyName("os_family")]
        public string OsFamily { get; set; } = string.Empty;
    }
}
