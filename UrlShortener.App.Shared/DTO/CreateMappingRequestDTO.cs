namespace UrlShortener.App.Shared.Dto
{
    public class CreateMappingRequestDto
    {
        public string LongUrl { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
    }
}
