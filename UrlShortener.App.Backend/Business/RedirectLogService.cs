using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Service for logging redirects.
    /// </summary>
    /// <param name="DbContext"></param>
    internal class RedirectLogService(AppDbContext DbContext) : IRedirectLogService
    {
        /// <inheritdoc />
        public async Task LogRedirectAsync(UrlMapping urlMapping, UserAgentApiResponse? userAgentApiResponse, string? ipAddress, string? userAgent)
        {
            var redirectLog = new RedirectLog
            {
                UrlMappingId = urlMapping.Id,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                AccessedAt = DateTime.UtcNow,
                BrowserFamily = userAgentApiResponse?.BrowserFamily,
                ClientEngine = userAgentApiResponse?.Client?.Engine,
                ClientName = userAgentApiResponse?.Client?.Name,
                ClientType = userAgentApiResponse?.Client?.Type,
                DeviceBrand = userAgentApiResponse?.Device?.Brand,
                DeviceModel = userAgentApiResponse?.Device?.Model,
                DeviceType = userAgentApiResponse?.Device?.Type,
                OsName = userAgentApiResponse?.Os?.Name,
                OsVersion = userAgentApiResponse?.Os?.Version,
                OsFamily = userAgentApiResponse?.OsFamily
            };

            DbContext.RedirectLogs.Add(redirectLog);
            await DbContext.SaveChangesAsync();
        }
    }
}
