using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents a data transfer object for device type statistics of URL mappings.
    /// </summary>
    public class DeviceTypeDataPointDto
    {
        /// <summary>
        /// Device type (e.g., "Desktop", "Mobile", "Tablet")
        /// </summary>
        [JsonPropertyName("deviceType")]
        public string DeviceType { get; set; }

        /// <summary>
        /// Number of clicks
        /// </summary>
        [JsonPropertyName("clicks")]
        public int Clicks { get; set; }
    }
}
