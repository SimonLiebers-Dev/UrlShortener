using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public interface IMappingsService
    {
        Task<UrlMapping?> CreateMapping(string longUrl, string? email = null);
        Task<UrlMapping?> GetMappingByPath(string path);
        Task<List<UrlMapping>?> GetMappingsByUser(string email);
    }
}
