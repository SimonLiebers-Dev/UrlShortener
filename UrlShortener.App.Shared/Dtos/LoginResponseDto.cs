using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class LoginResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
