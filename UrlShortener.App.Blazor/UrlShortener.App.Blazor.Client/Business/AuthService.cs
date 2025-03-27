using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UrlShortener.App.Blazor.Client.Api;

namespace UrlShortener.App.Blazor.Client.Business
{
    public class AuthService(IAuthApi AuthApi, INotificationService NotificationService, AuthenticationStateProvider AuthenticationStateProvider, NavigationManager NavigationManager) : IAuthService
    {
        public async Task LoginAsync(string username, string password)
        {
            try
            {
                var token = await AuthApi.Login(username, password);
                if (token != null)
                {
                    await ((AppAuthenticationStateProvider)AuthenticationStateProvider).TryMarkUserAsAuthenticated(token);
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    await NotificationService.Error("Email address or password wrong");
                }
            }
            catch (Exception ex)
            {
                await NotificationService.Error("Authentication service not available. Try again later.");
            }
        }

        public async Task RegisterAsync(string username, string password)
        {
            try
            {
                var success = await AuthApi.Register(username, password);
            }
            catch
            {
                await NotificationService.Error("Authentication service not available. Try again later.");
            }
        }
    }
}
