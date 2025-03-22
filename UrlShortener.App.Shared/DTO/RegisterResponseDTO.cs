namespace UrlShortener.App.Shared.Dto
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; } = true;
        public RegisterErrorType ErrorType { get; set; } = RegisterErrorType.None;
    }
}
