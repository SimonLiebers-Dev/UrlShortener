namespace UrlShortener.App.Shared.Models
{
    /// <summary>
    /// Represents a registered user in the system, including authentication credentials.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user. Used as the login identifier.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cryptographic salt used to hash the user's password.
        /// </summary>
        public string Salt { get; set; } = string.Empty;
    }
}
