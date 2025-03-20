using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IRedirectLogService
    {
        Task LogRedirectAsync(UrlMapping urlMapping, IpApiResponse? ipApiResponse, string? ipAddress, string? userAgent);
    }
}
