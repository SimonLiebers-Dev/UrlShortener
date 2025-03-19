namespace UrlShortener.App.Shared.Models
{
    public class RedirectLog
    {
        public int Id { get; set; }
        public int UrlMappingId { get; set; }
        public UrlMapping UrlMapping { get; set; } = null!;
        public string? IpAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? UserAgent { get; set; } = string.Empty;
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
    }
}
