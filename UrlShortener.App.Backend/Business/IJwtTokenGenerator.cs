namespace UrlShortener.App.Backend.Business
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT token for the given email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>JWT token</returns>
        public string GenerateToken(string email);
    }
}
