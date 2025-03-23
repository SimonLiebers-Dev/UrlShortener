using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    internal interface IUserAgentService
    {
        Task<UserAgentApiResponse?> GetUserAgentAsync(string userAgent);
    }
}
