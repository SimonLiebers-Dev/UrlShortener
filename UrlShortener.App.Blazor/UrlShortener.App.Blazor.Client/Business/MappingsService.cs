using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Business
{
    public class MappingsService(HttpClient HttpClient, AuthenticationStateProvider AuthenticationStateProvider, NavigationManager NavigationManager) : IMappingsService
    {
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

        private void SetupHttpClient()
        {
            var token = ((AppAuthenticationStateProvider)AuthenticationStateProvider).GetToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

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
