using System.Security.Claims;

namespace UrlShortener.App.Frontend.Business
{
    public interface IAuthService
    {
        Task<string?> Login(string email, string password);
        Task<bool> Register(string email, string password);
        Task Logout();
    }
}
