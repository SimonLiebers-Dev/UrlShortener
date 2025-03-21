using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IRedirectLogService
    {
        Task LogRedirectAsync(UrlMapping urlMapping, UserAgentApiResponse? userAgentApiResponse, string? ipAddress, string? userAgent);
    }
}
