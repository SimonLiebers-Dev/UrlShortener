using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class RegisterResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;

        [JsonPropertyName("error_type")]
        public RegisterErrorType ErrorType { get; set; } = RegisterErrorType.None;
    }
}
