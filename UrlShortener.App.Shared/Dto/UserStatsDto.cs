using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents aggregated statistics for a user, including total clicks and total URL mappings.
    /// </summary>
    public class UserStatsDto
    {
        /// <summary>
        /// Gets or sets the total number of redirect clicks across all of the user's URL mappings.
        /// </summary>
        [JsonPropertyName("clicks")]
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the total number of URL mappings created by the user.
        /// </summary>
        [JsonPropertyName("mappings")]
        public int Mappings { get; set; }

        /// <summary>
        /// Gets or sets the statistics related to the device types used for accessing the user's URL mappings.
        /// </summary>
        [JsonPropertyName("deviceTypeStats")]
        public IEnumerable<DeviceTypeDataPointDto> DeviceTypeStats { get; set; } = [];

        /// <summary>
        /// Gets or sets the statistics related to the time series of clicks on the user's URL mappings.
        /// </summary>
        [JsonPropertyName("timeSeriesStats")]
        public IEnumerable<TimeSeriesStatsDto> TimeSeriesStats { get; set; } = [];
    }
}
