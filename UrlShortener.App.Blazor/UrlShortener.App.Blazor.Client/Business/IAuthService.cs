namespace UrlShortener.App.Blazor.Client.Business
{
    public interface IAuthService
    {
        Task LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }
}
