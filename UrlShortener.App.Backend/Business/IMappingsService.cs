using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    internal interface IMappingsService
    {
        Task<UrlMapping?> CreateMapping(string longUrl, string name, string email);
        Task<UrlMapping?> GetMappingByPath(string path);
        Task<List<UrlMapping>?> GetMappingsByUser(string email);
        Task<bool> DeleteMapping(string email, int mappingId);
    }
}
