namespace UrlShortener.App.Shared.DTO
{
    public class UrlMappingDTO
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? User { get; set; }
        public List<RedirectLogDTO> RedirectLogs { get; set; } = [];
    }
}
