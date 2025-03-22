using System.Security.Claims;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Frontend.Business
{
    public interface IAuthService
    {
        Task<string?> Login(string email, string password);
        Task<RegisterResponseDto?> Register(string email, string password);
    }
}
