using System.Net.Http.Json;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public class AuthService(HttpClient HttpClient) : IAuthService
    {
        public async Task<string?> Login(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            return result?.Token;
        }

        public async Task<RegisterResponseDTO?> Register(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<RegisterResponseDTO>();
        }
    }
}
