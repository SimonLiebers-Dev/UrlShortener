using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the request payload for creating a new shortened URL mapping.
    /// </summary>
    public class CreateMappingRequestDto
    {
        /// <summary>
        /// Gets or sets the original long URL to be shortened.
        /// </summary>
        [JsonPropertyName("long_url")]
        public string LongUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an custom name or label for the URL mapping.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; } = string.Empty;
    }
}
