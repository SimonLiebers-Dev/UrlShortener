using System.Net.Http.Json;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public class MappingsService(HttpClient HttpClient) : IMappingsService
    {
        public async Task<List<UrlMappingDTO>?> GetMappings()
        {
            var response = await HttpClient.GetAsync("api/url/mappings");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<UrlMappingDTO>>();
        }

        public async Task<CreateMappingResponseDTO?> CreateMapping(string longUrl)
        {
            var response = await HttpClient.PostAsJsonAsync("api/url/create", new CreateMappingRequestDTO() { LongUrl = longUrl });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CreateMappingResponseDTO>();
        }
    }
}
