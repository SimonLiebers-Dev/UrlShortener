using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the data transfer object for time series statistics of URL mappings.
    /// </summary>
    public class TimeSeriesStatsDto
    {
        /// <summary>
        /// Represents the unique identifier for the URL mapping.
        /// </summary>
        [JsonPropertyName("id")]
        public int MappingId { get; set; }

        /// <summary>
        /// Represents the name of the URL mapping.
        /// </summary>
        [JsonPropertyName("name")]
        public string MappingName { get; set; }

        /// <summary>
        /// Represents the total number of clicks for the URL mapping per day.
        /// </summary>
        [JsonPropertyName("clicksPerDay")]
        public List<ClickDataPointDto> ClicksPerDay { get; set; } = [];
    }
}
