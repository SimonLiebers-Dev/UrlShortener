using System.Net.Http.Json;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public class AuthService(ILocalStorageService LocalStorageService, HttpClient HttpClient) : IAuthService
    {
        public async Task<string?> Login(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (result != null)
            {
                await LocalStorageService.SetItemAsync("authToken", result.Token);
                return result.Token;
            }
            return null;
        }

        public async Task Logout()
        {
            await LocalStorageService.RemoveItemAsync("authToken");
        }

        public async Task<bool> Register(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
            return response.IsSuccessStatusCode;
        }
    }
}
