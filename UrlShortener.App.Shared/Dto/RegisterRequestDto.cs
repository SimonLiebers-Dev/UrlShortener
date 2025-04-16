using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the request payload for registering a new user.
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// Gets the user's email address to register with.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Gets the user's chosen password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; init; } = string.Empty;
    }
}
