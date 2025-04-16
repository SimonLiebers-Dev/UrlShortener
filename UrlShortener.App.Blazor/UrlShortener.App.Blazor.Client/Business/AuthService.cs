using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Implementation of <see cref="IAuthService"/> that handles authentication and registration logic.
    /// Coordinates API calls, authentication state updates, navigation, and user notifications.
    /// </summary>
    /// <remarks>
    /// Depends on <see cref="IAuthApi"/> for backend communication, <see cref="INotificationService"/> for user feedback,
    /// <see cref="AuthenticationStateProvider"/> for authentication state management, and <see cref="NavigationManager"/> for routing.
    /// </remarks>
    public class AuthService(IAuthApi AuthApi, INotificationService NotificationService, AuthenticationStateProvider AuthenticationStateProvider, NavigationManager NavigationManager) : IAuthService
    {
        /// <summary>
        /// Attempts to log in the user with the provided credentials.
        /// If successful, updates the authentication state and navigates to the home page.
        /// Otherwise, shows an error notification.
        /// </summary>
        /// <param name="username">The user's email or username.</param>
        /// <param name="password">The user's password.</param>
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

        /// <summary>
        /// Attempts to register a new user with the provided credentials.
        /// On success, redirects to the home page.
        /// On failure, displays an appropriate error message.
        /// </summary>
        /// <param name="username">The user's email or username.</param>
        /// <param name="password">The user's password.</param>
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
