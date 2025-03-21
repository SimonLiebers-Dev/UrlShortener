using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace UrlShortener.App.Frontend.Business
{
    public class AppAuthenticationStateProvider(ILocalStorageService LocalStorageService, HttpClient HttpClient) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await LocalStorageService.GetItemAsync("authToken");

            var identity = new ClaimsIdentity();
            if (token != null)
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = WebEncoders.Base64UrlDecode(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null)
                return [];

            return keyValuePairs.Select(k => new Claim(k.Key, k.Value.ToString() ?? string.Empty));
        }

        public async Task TriggerLoginAsync(string token)
        {
            await LocalStorageService.SetItemAsync("authToken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task TriggerLogoutAsync()
        {
            await LocalStorageService.RemoveItemAsync("authToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
