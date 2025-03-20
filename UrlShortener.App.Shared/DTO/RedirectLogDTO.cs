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
