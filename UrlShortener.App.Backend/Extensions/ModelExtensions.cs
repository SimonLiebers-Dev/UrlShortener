using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Backend.Extensions
{
    public static class ModelExtensions
    {
        public static UserAgentInfoDto ToDto(this UserAgentApiResponse userAgentResponse)
        {
            return new UserAgentInfoDto()
            {
                BrowserFamily = userAgentResponse.BrowserFamily,
                Client = userAgentResponse.Client?.ToDto(),
                Device = userAgentResponse.Device?.ToDto(),
                Os = userAgentResponse.Os?.ToDto(),
                OsFamily = userAgentResponse.OsFamily
            };
        }

        public static UserAgentClientDto ToDto(this UserAgentClient userAgentClient)
        {
            return new UserAgentClientDto()
            {
                Engine = userAgentClient.Engine,
                EngineVersion = userAgentClient.EngineVersion,
                Name = userAgentClient.Name,
                Type = userAgentClient.Type,
                Version = userAgentClient.Version
            };
        }

        public static UserAgentDeviceDto ToDto(this UserAgentDevice userAgentDevice)
        {
            return new UserAgentDeviceDto()
            {
                Brand = userAgentDevice.Brand,
                Model = userAgentDevice.Model,
                Type = userAgentDevice.Type
            };
        }

        public static UserAgentOsDto ToDto(this UserAgentOs userAgentOs)
        {
            return new UserAgentOsDto()
            {
                Name = userAgentOs.Name,
                Platform = userAgentOs.Platform,
                Version = userAgentOs.Version
            };
        }
    }
}
