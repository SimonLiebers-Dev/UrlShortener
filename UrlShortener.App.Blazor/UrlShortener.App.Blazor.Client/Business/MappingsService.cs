using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Implementation of <see cref="IMappingsService"/> that communicates with the backend to manage URL mappings.
    /// Handles creation, retrieval, deletion, and statistics via authenticated HTTP requests.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="HttpClient"/> for API calls, <see cref="AuthenticationStateProvider"/> for JWT access,
    /// and <see cref="NavigationManager"/> for redirecting unauthorized users.
    /// </remarks>
    public class MappingsService(HttpClient HttpClient, AuthenticationStateProvider AuthenticationStateProvider, NavigationManager NavigationManager) : IMappingsService
    {
        /// <inheritdoc />
        public async Task<List<UrlMappingDto>?> GetMappings()
        {
            var response = await HttpClient.GetAsync("api/mappings/all");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<UrlMappingDto>>();
        }

        /// <inheritdoc />
        public async Task<CreateMappingResponseDto?> CreateMapping(string longUrl, string? name = null)
        {
            SetupHttpClient();

            var response = await HttpClient.PostAsJsonAsync("api/mappings/create", new CreateMappingRequestDto() { Name = name, LongUrl = longUrl });

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CreateMappingResponseDto>();
        }

        /// <inheritdoc />
        public async Task<UserStatsDto?> GetStats()
        {
            SetupHttpClient();

            var response = await HttpClient.GetAsync("api/mappings/stats");

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserStatsDto>();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteMapping(UrlMappingDto mapping)
        {
            SetupHttpClient();

            var response = await HttpClient.DeleteAsync($"api/mappings/{mapping.Id}");
            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
            }
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Sets the authorization header on the <see cref="HttpClient"/> using the current JWT token.
        /// </summary>
        private void SetupHttpClient()
        {
            var token = ((AppAuthenticationStateProvider)AuthenticationStateProvider).GetToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Handles HTTP error responses. If unauthorized, logs out the user and redirects to the login page.
        /// </summary>
        /// <param name="message">The HTTP response message to evaluate.</param>
        private async Task HandleErrorResponse(HttpResponseMessage message)
        {
            if (message == null)
                return;

            if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ((AppAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();
                NavigationManager.NavigateTo("/auth", true);
            }
        }
    }
}
