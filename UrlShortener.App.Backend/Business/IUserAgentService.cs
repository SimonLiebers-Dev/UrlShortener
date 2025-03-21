using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IUserAgentService
    {
        Task<UserAgentApiResponse?> GetUserAgentAsync(string userAgent);
    }
}
