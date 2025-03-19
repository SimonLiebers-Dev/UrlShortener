using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IRedirectLogService
    {
        Task LogRedirectAsync(UrlMapping urlMapping, string? ipAddress, string? userAgent, double? latitude, double? longitude);
    }
}
