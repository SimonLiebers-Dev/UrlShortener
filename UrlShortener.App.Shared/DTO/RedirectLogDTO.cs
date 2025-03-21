namespace UrlShortener.App.Shared.DTO
{
    public class RedirectLogDTO
    {
        public int Id { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
        public string? BrowserFamily { get; set; }
        public string? ClientEngine { get; set; }
        public string? ClientName { get; set; }
        public string? ClientType { get; set; }
        public string? DeviceBrand { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceType { get; set; }
        public string? OsName { get; set; }
        public string? OsVersion { get; set; }
        public string? OsFamily { get; set; }
    }
}
