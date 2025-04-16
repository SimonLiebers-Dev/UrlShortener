using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    /// <summary>
    /// Represents the response returned after a user registration attempt.
    /// Indicates whether registration was successful and, if not, the type of error encountered.
    /// </summary>
    public class RegisterResponseDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registration was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of error that occurred during registration, if any.
        /// </summary>
        [JsonPropertyName("error_type")]
        public RegisterErrorType ErrorType { get; set; } = RegisterErrorType.None;
    }
}
