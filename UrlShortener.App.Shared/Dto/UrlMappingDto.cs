using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents a shortened URL mapping along with its metadata and redirect logs.
    /// </summary>
    public class UrlMappingDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the URL mapping.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the original long URL.
        /// </summary>
        [JsonPropertyName("long_url")]
        public string LongUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional name or label associated with this URL mapping.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the generated short URL.
        /// </summary>
        [JsonPropertyName("short_url")]
        public string ShortUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the mapping was created (in UTC).
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the email address or identifier of the user who created the mapping.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

        /// <summary>
        /// Gets or sets the collection of redirect logs associated with this mapping.
        /// </summary>
        [JsonPropertyName("redirect_logs")]
        public List<RedirectLogDto> RedirectLogs { get; set; } = [];
    }
}
