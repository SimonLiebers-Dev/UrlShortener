using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IIpLookupService
    {
        Task<IpApiResponse?> GetDataAsync(string? ip);
    }
}
