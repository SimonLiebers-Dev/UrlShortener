using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Provides functionality for retrieving and parsing user agent information from a user’s browser.
    /// </summary>
    public interface IUserAgentService
    {
        /// <summary>
        /// Retrieves and parses user agent data from the provided user agent string.
        /// </summary>
        /// <param name="userAgent">The raw user agent string from the user's browser.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the parsed
        /// <see cref="UserAgentApiResponse"/>, or <c>null</c> if parsing failed or no data was returned.
        /// </returns>
        Task<UserAgentApiResponse?> GetUserAgentAsync(string userAgent);
    }
}
