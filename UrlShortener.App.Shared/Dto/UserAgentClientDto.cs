using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents details about the client software (e.g., browser or app) extracted from a user agent string.
    /// </summary>
    public class UserAgentClientDto
    {
        /// <summary>
        /// Gets or sets the rendering engine used by the client (e.g., Blink, Gecko).
        /// </summary>
        [JsonPropertyName("engine")]
        public string Engine { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the rendering engine.
        /// </summary>
        [JsonPropertyName("engine_version")]
        public string EngineVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the client software (e.g., Chrome, Safari).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of client (e.g., browser, mobile app).
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
