namespace UrlShortener.App.Shared.Models
{
    /// <summary>
    /// Represents a log entry for a redirection event, including metadata about the request and client environment.
    /// </summary>
    public class RedirectLog
    {
        /// <summary>
        /// Gets or sets the unique identifier of the redirect log entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated <see cref="UrlMapping"/>.
        /// </summary>
        public int UrlMappingId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated <see cref="UrlMapping"/>.
        /// </summary>
        public UrlMapping UrlMapping { get; set; } = null!;

        /// <summary>
        /// Gets or sets the IP address of the user who triggered the redirect.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the raw user agent string from the user's browser.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the redirect occurred (in UTC).
        /// </summary>
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the browser family (e.g., Chrome, Firefox).
        /// </summary>
        public string? BrowserFamily { get; set; }

        /// <summary>
        /// Gets or sets the name of the rendering engine used by the client (e.g., Blink, Gecko).
        /// </summary>
        public string? ClientEngine { get; set; }

        /// <summary>
        /// Gets or sets the name of the client software (e.g., Chrome, Safari).
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the type of the client (e.g., browser, app).
        /// </summary>
        public string? ClientType { get; set; }

        /// <summary>
        /// Gets or sets the brand of the user's device (e.g., Apple, Samsung).
        /// </summary>
        public string? DeviceBrand { get; set; }

        /// <summary>
        /// Gets or sets the model of the user's device (e.g., iPhone 14, Galaxy S23).
        /// </summary>
        public string? DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the type of the user's device (e.g., smartphone, tablet, desktop).
        /// </summary>
        public string? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operating system (e.g., Windows, iOS).
        /// </summary>
        public string? OsName { get; set; }

        /// <summary>
        /// Gets or sets the version of the operating system.
        /// </summary>
        public string? OsVersion { get; set; }

        /// <summary>
        /// Gets or sets the operating system family (e.g., Windows, Linux, macOS).
        /// </summary>
        public string? OsFamily { get; set; }
    }
}
