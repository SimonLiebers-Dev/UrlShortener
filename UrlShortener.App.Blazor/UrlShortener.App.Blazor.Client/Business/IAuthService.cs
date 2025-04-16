namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Defines authentication operations, such as logging in and registering users.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="username">The user's email or username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LoginAsync(string username, string password);

        /// <summary>
        /// Attempts to register a new user with the provided credentials.
        /// </summary>
        /// <param name="username">The user's email or username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RegisterAsync(string username, string password);
    }
}
