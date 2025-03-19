namespace UrlShortener.App.Shared.Models
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string Path { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? User { get; set; }
        public List<RedirectLog> RedirectLogs { get; set; } = [];
    }
}
