using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Provides functionality for logging redirect events for shortened URLs.
    /// </summary>
    public interface IRedirectLogService
    {
        /// <summary>
        /// Logs a redirect event, capturing metadata such as the user's IP address, user agent, and device information.
        /// </summary>
        /// <param name="urlMapping">The URL mapping associated with the redirection.</param>
        /// <param name="userAgentApiResponse">Optional parsed user agent data, such as browser and device info.</param>
        /// <param name="ipAddress">Optional IP address of the user initiating the redirect.</param>
        /// <param name="userAgent">Optional raw user agent string of the user's browser.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        Task LogRedirectAsync(UrlMapping urlMapping, UserAgentApiResponse? userAgentApiResponse, string? ipAddress, string? userAgent);
    }
}
