using Microsoft.AspNetCore.Http;
using UrlShortener.App.Shared.DTO;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Shared.Extensions
{
    public static class DtoExtensions
    {
        public static UrlMappingDTO ToDTO(this UrlMapping urlMapping, HttpRequest httpRequest)
        {
            return new UrlMappingDTO()
            {
                Id = urlMapping.Id,
                LongUrl = urlMapping.LongUrl,
                Name = urlMapping.Name,
                ShortUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{urlMapping.Path}",
                CreatedAt = urlMapping.CreatedAt,
                User = urlMapping.User,
                RedirectLogs = [.. urlMapping.RedirectLogs.Select(log => log.ToDTO())]
            };
        }

        public static RedirectLogDTO ToDTO(this RedirectLog redirectLog)
        {
            return new RedirectLogDTO()
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
