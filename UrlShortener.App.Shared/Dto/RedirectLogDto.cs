using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents a log entry for a URL redirection, including access metadata and client details.
    /// </summary>
    public class RedirectLogDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the redirect log entry.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the user who triggered the redirect.
        /// </summary>
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the raw user agent string from the user's browser.
        /// </summary>
        [JsonPropertyName("user_agent")]
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the redirect occurred.
        /// </summary>
        [JsonPropertyName("accessed_at")]
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the browser family name (e.g., Chrome, Firefox).
        /// </summary>
        [JsonPropertyName("browser_family")]
        public string? BrowserFamily { get; set; }

        /// <summary>
        /// Gets or sets the engine used by the client (e.g., Blink, Gecko).
        /// </summary>
        [JsonPropertyName("client_engine")]
        public string? ClientEngine { get; set; }

        /// <summary>
        /// Gets or sets the name of the client (e.g., Chrome, Safari).
        /// </summary>
        [JsonPropertyName("client_name")]
        public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the type of client (e.g., browser, app).
        /// </summary>
        [JsonPropertyName("client_type")]
        public string? ClientType { get; set; }

        /// <summary>
        /// Gets or sets the brand of the device (e.g., Apple, Samsung).
        /// </summary>
        [JsonPropertyName("device_brand")]
        public string? DeviceBrand { get; set; }

        /// <summary>
        /// Gets or sets the model of the device (e.g., iPhone 13, Galaxy S21).
        /// </summary>
        [JsonPropertyName("device_model")]
        public string? DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the type of device (e.g., smartphone, tablet, desktop).
        /// </summary>
        [JsonPropertyName("device_type")]
        public string? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operating system (e.g., Windows, iOS).
        /// </summary>
        [JsonPropertyName("os_name")]
        public string? OsName { get; set; }

        /// <summary>
        /// Gets or sets the version of the operating system.
        /// </summary>
        [JsonPropertyName("os_version")]
        public string? OsVersion { get; set; }

        /// <summary>
        /// Gets or sets the operating system family (e.g., Windows, macOS, Linux).
        /// </summary>
        [JsonPropertyName("os_family")]
        public string? OsFamily { get; set; }
    }
}
