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
            return new UrlMappingDto()
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
            return new RedirectLogDto()
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
    }
}
