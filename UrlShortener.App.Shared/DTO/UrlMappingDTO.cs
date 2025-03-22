namespace UrlShortener.App.Shared.Dto
{
    public class UrlMappingDto
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? User { get; set; }
        public List<RedirectLogDto> RedirectLogs { get; set; } = [];
    }
}
