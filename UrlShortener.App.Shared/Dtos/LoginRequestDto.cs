using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class LoginRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; init; } = string.Empty;
    }
}
