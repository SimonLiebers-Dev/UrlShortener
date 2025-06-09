using Microsoft.AspNetCore.Http;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Shared.Extensions
{
    /// <summary>
    /// Provides extension methods for converting entity models to their corresponding DTO representations.
    /// </summary>
    public static class DtoExtensions
    {
        /// <summary>
        /// Converts a <see cref="UrlMapping"/> entity to a <see cref="UrlMappingDto"/> for use in API responses.
        /// </summary>
        /// <param name="urlMapping">The URL mapping entity to convert.</param>
        /// <param name="httpRequest">The current HTTP request, used to build the full short URL.</param>
        /// <returns>A <see cref="UrlMappingDto"/> representing the URL mapping.</returns>
        public static UrlMappingDto ToDto(this UrlMapping urlMapping, HttpRequest httpRequest)
        {
            return new UrlMappingDto
            {
                Id = urlMapping.Id,
                LongUrl = urlMapping.LongUrl,
                Name = urlMapping.Name,
                ShortUrl = $"{httpRequest.Scheme}://localhost:{httpRequest.Host.Port}/{urlMapping.Path}",
                CreatedAt = urlMapping.CreatedAt,
                User = urlMapping.User,
                RedirectLogs = [.. urlMapping.RedirectLogs.Select(log => log.ToDto())]
            };
        }

        /// <summary>
        /// Converts a <see cref="RedirectLog"/> entity to a <see cref="RedirectLogDto"/> for use in API responses.
        /// </summary>
        /// <param name="redirectLog">The redirect log entity to convert.</param>
        /// <returns>A <see cref="RedirectLogDto"/> representing the redirect log.</returns>
        public static RedirectLogDto ToDto(this RedirectLog redirectLog)
        {
            return new RedirectLogDto
            {
                Id = redirectLog.Id,
                IpAddress = redirectLog.IpAddress,
                UserAgent = redirectLog.UserAgent,
                AccessedAt = redirectLog.AccessedAt,
                BrowserFamily = redirectLog.BrowserFamily,
                ClientEngine = redirectLog.ClientEngine,
                ClientName = redirectLog.ClientName,
                ClientType = redirectLog.ClientType,
                DeviceBrand = redirectLog.DeviceBrand,
                DeviceModel = redirectLog.DeviceModel,
                DeviceType = redirectLog.DeviceType,
                OsName = redirectLog.OsName,
                OsVersion = redirectLog.OsVersion,
                OsFamily = redirectLog.OsFamily
            };
        }

        /// <summary>
        /// Converts a collection of <see cref="UrlMapping"/> entities to a <see cref="UserStatsDto"/> containing aggregated statistics.
        /// </summary>
        /// <param name="userMappings">List of mappings</param>
        /// <returns>A <see cref="UserStatsDto"/> containing statistics about the user's URL mappings.</returns>
        public static UserStatsDto GetStats(this IEnumerable<UrlMapping> userMappings)
        {
            var deviceTypeStats = userMappings.SelectMany(s => s.RedirectLogs)
                    .GroupBy(log => log.DeviceType ?? "Unknown")
                    .Select(g => new DeviceTypeDataPointDto
                    {
                        DeviceType = g.Key,
                        Clicks = g.Count(),
                    })
                    .ToList();

            var timeSeriesStats = new List<TimeSeriesStatsDto>();
            foreach (var mapping in userMappings)
            {
                var item = new TimeSeriesStatsDto
                {
                    MappingId = mapping.Id,
                    MappingName = mapping.Name ?? "Unknown",
                    ClicksPerDay = [.. mapping.RedirectLogs
                        .GroupBy(log => log.AccessedAt.Date)
                        .Select(g => new ClickDataPointDto
                        {
                            DateTime = g.Key,
                            Clicks = g.Count()
                        })]
                };
                timeSeriesStats.Add(item);
            }

            return new UserStatsDto
            {
                Clicks = userMappings.SelectMany(s => s.RedirectLogs).Count(),
                Mappings = userMappings.Count(),
                DeviceTypeStats = deviceTypeStats,
                TimeSeriesStats = timeSeriesStats
            };
        }
    }
}
