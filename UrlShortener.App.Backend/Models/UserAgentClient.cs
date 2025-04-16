using System.Text.Json.Serialization;

namespace UrlShortener.App.Backend.Models
{
    /// <summary>
    /// Represents detailed information about the client software making the request,
    /// such as a browser or app, extracted from the user agent string.
    /// </summary>
    public class UserAgentClient
    {
        /// <summary>
        /// Gets or sets the name of the rendering engine used by the client (e.g., Blink, Gecko).
        /// </summary>
        [JsonPropertyName("engine")]
        public string Engine { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the rendering engine.
        /// </summary>
        [JsonPropertyName("engine_version")]
        public string EngineVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the client (e.g., Chrome, Firefox, Safari).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of client (e.g., browser, mobile app, feed reader).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the client software.
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}
