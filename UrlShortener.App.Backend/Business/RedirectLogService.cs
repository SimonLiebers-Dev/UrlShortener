using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public class RedirectLogService(AppDbContext DbContext) : IRedirectLogService
    {
        public async Task LogRedirectAsync(UrlMapping urlMapping, string? ipAddress, string? userAgent, double? latitude, double? longitude)
        {
            var redirectLog = new RedirectLog
            {
                UrlMappingId = urlMapping.Id,
                AccessedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Latitude = latitude,
                Longitude = longitude
            };

            DbContext.RedirectLogs.Add(redirectLog);
            await DbContext.SaveChangesAsync();
        }
    }
}
