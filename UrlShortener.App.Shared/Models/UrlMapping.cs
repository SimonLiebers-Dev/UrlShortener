namespace UrlShortener.App.Shared.Models
{
    /// <summary>
    /// Represents a shortened URL mapping, including the original URL, its short path, metadata, and related redirect logs.
    /// </summary>
    public class UrlMapping
    {
        /// <summary>
        /// Gets or sets the unique identifier of the URL mapping.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the original long URL that was shortened.
        /// </summary>
        public string LongUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional name or label for the mapping.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the unique short path used for redirection.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UTC timestamp when the mapping was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the mapping (e.g., email address).
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// Gets or sets the collection of redirect logs associated with this URL mapping.
        /// </summary>
        public List<RedirectLog> RedirectLogs { get; set; } = [];
    }
}
