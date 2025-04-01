using System.Text.Json.Serialization;

namespace UrlShortener.App.Shared.Dto
{
    public class RedirectLogDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        [JsonPropertyName("user_agent")]
        public string? UserAgent { get; set; }

        [JsonPropertyName("accessed_at")]
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("browser_family")]
        public string? BrowserFamily { get; set; }

        [JsonPropertyName("client_engine")]
        public string? ClientEngine { get; set; }

        [JsonPropertyName("client_name")]
        public string? ClientName { get; set; }

        [JsonPropertyName("client_type")]
        public string? ClientType { get; set; }

        [JsonPropertyName("device_brand")]
        public string? DeviceBrand { get; set; }

        [JsonPropertyName("device_model")]
        public string? DeviceModel { get; set; }

        [JsonPropertyName("device_type")]
        public string? DeviceType { get; set; }

        [JsonPropertyName("os_name")]
        public string? OsName { get; set; }

        [JsonPropertyName("os_version")]
        public string? OsVersion { get; set; }

        [JsonPropertyName("os_family")]
        public string? OsFamily { get; set; }
    }
}
