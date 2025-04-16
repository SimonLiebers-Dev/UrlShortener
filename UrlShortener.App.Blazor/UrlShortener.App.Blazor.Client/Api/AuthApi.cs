using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Api
{
    /// <summary>
    /// Implementation of <see cref="IAuthApi"/> that communicates with the backend authentication API using <see cref="HttpClient"/>.
    /// </summary>
    public class AuthApi(HttpClient HttpClient) : IAuthApi
    {
        /// <summary>
        /// Sends a login request to the API with the specified email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The result contains a JWT token string if the login is successful; otherwise, <c>null</c>.
        /// </returns>
        public async Task<string?> Login(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return result?.Token;
        }

        /// <summary>
        /// Sends a registration request to the API with the specified email and password.
        /// </summary>
        /// <param name="email">The email address for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The result contains a <see cref="RegisterResponseDto"/> with the outcome of the registration, or <c>null</c> if parsing fails.
        /// </returns>
        public async Task<RegisterResponseDto?> Register(string email, string password)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });

            try
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
            }
            catch
            {
                return null;
            }
        }
    }
}
