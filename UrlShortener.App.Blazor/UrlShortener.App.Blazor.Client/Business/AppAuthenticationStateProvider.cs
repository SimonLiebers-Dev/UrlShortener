using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Provides the authentication state for the Blazor application using JWT stored in sessionStorage.
    /// Inherits from <see cref="AuthenticationStateProvider"/>.
    /// </summary>
    public class AppAuthenticationStateProvider(IJSRuntime JsRuntime) : AuthenticationStateProvider
    {
        private string? _token = null;

        /// <summary>
        /// Gets the current authentication state, including user claims if a valid JWT token is present.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The result contains the current <see cref="AuthenticationState"/>.
        /// </returns>
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

        /// <summary>
        /// Returns the current JWT token.
        /// </summary>
        /// <returns>The JWT token as a string, or <c>null</c> if not set.</returns>
        public string? GetToken()
        {
            return _token;
        }

        /// <summary>
        /// Sets the authentication token, persists it to sessionStorage, and notifies the application of the updated auth state.
        /// </summary>
        /// <param name="token">The JWT token to set.</param>
        public async Task TryMarkUserAsAuthenticated(string? token)
        {
            _token = token;

            await JsRuntime.InvokeVoidAsync("sessionStorage.setItem", "authToken", _token ?? string.Empty);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Clears the current authentication token, removes it from sessionStorage, and notifies the application of the logout.
        /// </summary>
        public async Task MarkUserAsLoggedOut()
        {
            _token = null;

            await JsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "authToken");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Parses a JWT token and extracts claims from its payload.
        /// </summary>
        /// <param name="jwt">The JWT token string.</param>
        /// <returns>A collection of claims extracted from the token.</returns>
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
