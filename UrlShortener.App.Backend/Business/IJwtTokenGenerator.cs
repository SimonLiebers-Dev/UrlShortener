namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Provides functionality for generating JSON Web Tokens (JWT).
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT token for the specified email address.
        /// </summary>
        /// <param name="email">The email address for which to generate the token.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateToken(string email);
    }
}
