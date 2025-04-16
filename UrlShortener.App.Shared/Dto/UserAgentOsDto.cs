using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents information about the operating system extracted from the user agent string.
    /// </summary>
    public class UserAgentOsDto
    {
        /// <summary>
        /// Gets or sets the name of the operating system (e.g., Windows, macOS, Android).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the platform or architecture of the OS (e.g., x86_64, ARM).
        /// </summary>
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the operating system.
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}
