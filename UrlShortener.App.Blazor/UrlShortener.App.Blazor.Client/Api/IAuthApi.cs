using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Api
{
    public interface IAuthApi
    {
        Task<string?> Login(string email, string password);
        Task<RegisterResponseDto?> Register(string email, string password);
    }
}
