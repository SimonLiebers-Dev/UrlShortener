using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IRedirectLogService
    {
        /// <summary>
        /// Log a redirect
        /// </summary>
        /// <param name="urlMapping">UrlMapping to create log for</param>
        /// <param name="userAgentApiResponse">User agent data of the user</param>
        /// <param name="ipAddress">Ip address of the user</param>
        /// <param name="userAgent">UserAgent of the users browser</param>
        /// <returns></returns>
        Task LogRedirectAsync(UrlMapping urlMapping, UserAgentApiResponse? userAgentApiResponse, string? ipAddress, string? userAgent);
    }
}
