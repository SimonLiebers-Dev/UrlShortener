using System.Security.Claims;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Frontend.Business
{
    public interface IAuthService
    {
        Task<string?> Login(string email, string password);
        Task<RegisterResponseDTO?> Register(string email, string password);
    }
}
