using System.Net.Http.Json;
using System.Xml.Linq;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public class MappingsService(HttpClient HttpClient) : IMappingsService
    {
        public async Task<List<UrlMappingDTO>?> GetMappings()
        {
            var response = await HttpClient.GetAsync("api/mappings/all");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<UrlMappingDTO>>();
        }

        public async Task<CreateMappingResponseDTO?> CreateMapping(string longUrl, string? name = null)
        {
            var response = await HttpClient.PostAsJsonAsync("api/mappings/create", new CreateMappingRequestDTO() { Name = name, LongUrl = longUrl });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CreateMappingResponseDTO>();
        }

        public async Task<UserStatsDTO?> GetStats()
        {
            var response = await HttpClient.GetAsync("api/mappings/stats");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserStatsDTO>();
        }
    }
}
