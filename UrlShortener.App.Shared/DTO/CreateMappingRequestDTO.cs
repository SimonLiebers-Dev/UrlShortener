namespace UrlShortener.App.Shared.DTO
{
    public class CreateMappingRequestDTO
    {
        public string LongUrl { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
    }
}
