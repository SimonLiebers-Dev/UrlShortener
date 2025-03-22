using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Frontend.Business
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
            var response = await HttpClient.DeleteAsync($"api/mappings/{mapping.Id}");
            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
            }
            return response.IsSuccessStatusCode;
        }

        private async Task HandleErrorResponse(HttpResponseMessage message)
        {
            if (message == null)
                return;

            if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ((AppAuthenticationStateProvider)AuthenticationStateProvider).TriggerLogoutAsync();
                NavigationManager.NavigateTo("/login", true);
            }
        }
    }
}
