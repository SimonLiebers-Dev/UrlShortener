using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public interface IMappingsService
    {
        Task<List<UrlMappingDTO>?> GetMappings();
        Task<CreateMappingResponseDTO?> CreateMapping(string longUrl);
    }
}
