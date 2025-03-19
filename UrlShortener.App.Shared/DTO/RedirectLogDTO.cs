namespace UrlShortener.App.Shared.DTO
{
    public class RedirectLogDTO
    {
        public int Id { get; set; }
        public string? IpAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? UserAgent { get; set; } = string.Empty;
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
    }
}
