using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Shared.Dto;

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
            catch
            {
                await NotificationService.Error("Authentication service not available. Try again later.");
            }
        }

        public async Task RegisterAsync(string username, string password)
        {
            try
            {
                var response = await AuthApi.Register(username, password);
                if (response != null)
                {
                    if (response.Success)
                    {
                        NavigationManager.NavigateTo("/", true);
                    }
                    else
                    {
                        if (response.ErrorType == RegisterErrorType.EmailAlreadyExists)
                        {
                            await NotificationService.Error("Email already exists.");
                        }
                        else if (response.ErrorType == RegisterErrorType.MissingEmailOrPassword)
                        {
                            await NotificationService.Error("Please provide email and password.");
                        }
                    }
                }
                else
                {
                    await NotificationService.Error("Authentication service not available. Try again later.");
                }
            }
            catch
            {
                await NotificationService.Error("Authentication service not available. Try again later.");
            }
        }
    }
}
