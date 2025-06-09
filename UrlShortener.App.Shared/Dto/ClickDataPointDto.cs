namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents a data point for click statistics, including the timestamp and number of clicks.
    /// </summary>
    public class ClickDataPointDto
    {
        /// <summary>
        /// Timestamp of click
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Number of clicks at this timestamp
        /// </summary>
        public int Clicks { get; set; }
    }
}
