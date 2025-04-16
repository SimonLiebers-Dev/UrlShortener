using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Api
{
    /// <summary>
    /// Defines methods for communicating with the authentication API,
    /// such as logging in and registering users.
    /// </summary>
    public interface IAuthApi
    {
        /// <summary>
        /// Sends a login request with the provided credentials and retrieves a JWT token if successful.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains the JWT token string if successful, or <c>null</c> if authentication fails.
        /// </returns>
        Task<string?> Login(string email, string password);

        /// <summary>
        /// Sends a registration request to create a new user account.
        /// </summary>
        /// <param name="email">The email address for the new account.</param>
        /// <param name="password">The password for the new account.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains the registration response,
        /// or <c>null</c> if the request fails.
        /// </returns>
        Task<RegisterResponseDto?> Register(string email, string password);
    }
}
