using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IUserAgentService
    {
        /// <summary>
        /// Get user agent information
        /// </summary>
        /// <param name="userAgent">UserAgent string from the users browser</param>
        /// <returns>Parsed data</returns>
        Task<UserAgentApiResponse?> GetUserAgentAsync(string userAgent);
    }
}
