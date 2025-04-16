using System.Text.Json.Serialization;

namespace UrlShortener.App.Backend.Models
{
    /// <summary>
    /// Represents information about the physical device extracted from the user agent string.
    /// </summary>
    public class UserAgentDevice
    {
        /// <summary>
        /// Gets or sets the brand of the device (e.g., Apple, Samsung, Google).
        /// </summary>
        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model of the device (e.g., iPhone 13, Galaxy S22).
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of device (e.g., smartphone, tablet, desktop).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
