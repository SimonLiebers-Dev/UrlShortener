using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace UrlShortener.App.Blazor.Client.Business
{
    public class AppAuthenticationStateProvider(IJSRuntime JsRuntime) : AuthenticationStateProvider
    {
        private string? _token = null;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                var identity = new ClaimsIdentity(ParseClaimsFromJwt(_token), "jwt");
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            }
            else
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }
        }

        public string? GetToken()
        {
            return _token;
        }

        public async Task TryMarkUserAsAuthenticated(string? token)
        {
            _token = token;

            await JsRuntime.InvokeVoidAsync("sessionStorage.setItem", "authToken", _token ?? string.Empty);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            _token = null;

            await JsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "authToken");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
    }
}
