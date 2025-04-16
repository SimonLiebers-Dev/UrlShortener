using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the response returned after a successful login request.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets the JWT token issued to the authenticated user.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
