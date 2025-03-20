using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public class RedirectLogService(AppDbContext DbContext) : IRedirectLogService
    {
        public async Task LogRedirectAsync(UrlMapping urlMapping, IpApiResponse? ipApiResponse, string? ipAddress, string? userAgent)
        {
            var redirectLog = new RedirectLog
            {
                UrlMappingId = urlMapping.Id,
                AccessedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Latitude = ipApiResponse?.Lat,
                Longitude = ipApiResponse?.Lon,
                Country = ipApiResponse?.Country,
                CountryCode = ipApiResponse?.CountryCode,
                Region = ipApiResponse?.Region,
                RegionName = ipApiResponse?.RegionName,
                City = ipApiResponse?.City,
                Zip = ipApiResponse?.Zip,
                Timezone = ipApiResponse?.Timezone,
                Isp = ipApiResponse?.Isp
            };

            DbContext.RedirectLogs.Add(redirectLog);
            await DbContext.SaveChangesAsync();
        }
    }
}
