namespace UrlShortener.App.Shared.DTO
{
    public class RegisterResponseDTO
    {
        public bool Success { get; set; } = true;
        public RegisterErrorType ErrorType { get; set; } = RegisterErrorType.None;
    }
}
