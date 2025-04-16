using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the request payload for logging in a user.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets the user's email address used for login.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Gets the user's password used for login.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; init; } = string.Empty;
    }
}
