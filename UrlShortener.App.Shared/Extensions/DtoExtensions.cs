using Microsoft.AspNetCore.Http;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Shared.Extensions
{
    public static class DtoExtensions
    {
        public static UrlMappingDto ToDto(this UrlMapping urlMapping, HttpRequest httpRequest)
        {
            return new UrlMappingDto()
            {
                Id = urlMapping.Id,
                LongUrl = urlMapping.LongUrl,
                Name = urlMapping.Name,
                ShortUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{urlMapping.Path}",
                CreatedAt = urlMapping.CreatedAt,
                User = urlMapping.User,
                RedirectLogs = [.. urlMapping.RedirectLogs.Select(log => log.ToDto())]
            };
        }

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
