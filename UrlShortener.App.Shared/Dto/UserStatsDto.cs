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
        public int Clicks { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of URL mappings created by the user.
        /// </summary>
        [JsonPropertyName("mappings")]
        public int Mappings { get; set; } = 0;
    }
}
