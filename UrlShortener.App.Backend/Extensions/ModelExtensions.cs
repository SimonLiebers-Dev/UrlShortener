using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Backend.Extensions
{
    public static class ModelExtensions
    {
        public static UserAgentInfoDTO ToDTO(this UserAgentApiResponse userAgentResponse)
        {
            return new UserAgentInfoDTO()
            {
                BrowserFamily = userAgentResponse.BrowserFamily,
                Client = userAgentResponse.Client?.ToDTO(),
                Device = userAgentResponse.Device?.ToDTO(),
                Os = userAgentResponse.Os?.ToDTO(),
                OsFamily = userAgentResponse.OsFamily
            };
        }

        public static UserAgentClientDTO ToDTO(this UserAgentClient userAgentClient)
        {
            return new UserAgentClientDTO()
            {
                Engine = userAgentClient.Engine,
                EngineVersion = userAgentClient.EngineVersion,
                Name = userAgentClient.Name,
                Type = userAgentClient.Type,
                Version = userAgentClient.Version
            };
        }

        public static UserAgentDeviceDTO ToDTO(this UserAgentDevice userAgentDevice)
        {
            return new UserAgentDeviceDTO()
            {
                Brand = userAgentDevice.Brand,
                Model = userAgentDevice.Model,
                Type = userAgentDevice.Type
            };
        }

        public static UserAgentOsDTO ToDTO(this UserAgentOs userAgentOs)
        {
            return new UserAgentOsDTO()
            {
                Name = userAgentOs.Name,
                Platform = userAgentOs.Platform,
                Version = userAgentOs.Version
            };
        }
    }
}
