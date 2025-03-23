using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Frontend.Business
{
    internal class AuthService(HttpClient HttpClient) : IAuthService
    {
        public async Task<string?> Login(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return result?.Token;
        }

        public async Task<RegisterResponseDto?> Register(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        }
    }
}
