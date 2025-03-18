using System.Net.Http.Json;
using UrlShortener.App.Shared.DTO;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Frontend.Business
{
    public class UrlService(HttpClient HttpClient) : IUrlService
    {
        public async Task<List<UrlMapping>?> GetMappingsByUser(string userId)
        {
            return null;
        }

        public async Task<ShortenResponseDTO?> ShortenUrlAsync(string longUrl)
        {
            var response = await HttpClient.PostAsJsonAsync("api/url/shorten", new ShortenRequestDTO() { LongUrl = longUrl });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ShortenResponseDTO>();
        }
    }
}
