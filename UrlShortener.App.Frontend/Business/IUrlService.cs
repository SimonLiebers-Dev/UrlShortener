using UrlShortener.App.Shared.DTO;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Frontend.Business
{
    public interface IUrlService
    {
        Task<List<UrlMapping>?> GetMappingsByUser(string userId);
        Task<ShortenResponseDTO?> ShortenUrlAsync(string longUrl);
    }
}
