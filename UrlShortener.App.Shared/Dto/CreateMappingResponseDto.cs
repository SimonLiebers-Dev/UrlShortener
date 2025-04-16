using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the response returned after successfully creating a shortened URL mapping.
    /// </summary>
    public class CreateMappingResponseDto
    {
        /// <summary>
        /// Gets or sets the generated short URL.
        /// </summary>
        [JsonPropertyName("short_url")]
        public string ShortUrl { get; set; } = string.Empty;
    }
}
