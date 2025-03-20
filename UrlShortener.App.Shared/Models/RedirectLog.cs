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
        public string? UserAgent { get; set; }
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? Region { get; set; }
        public string? RegionName { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? Timezone { get; set; }
        public string? Isp { get; set; }
    }
}
