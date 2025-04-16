using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents information about the user's physical device, extracted from the user agent string.
    /// </summary>
    public class UserAgentDeviceDto
    {
        /// <summary>
        /// Gets or sets the brand of the device (e.g., Apple, Samsung).
        /// </summary>
        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model of the device (e.g., iPhone 14, Galaxy S23).
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the device (e.g., smartphone, tablet, desktop).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
