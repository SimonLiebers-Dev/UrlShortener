using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class RedirectController(IMappingsService MappingsService, IRedirectLogService RedirectLogService, IIpLookupService IpLookupService) : ControllerBase
    {
        [HttpGet("{path}")]
        public async Task<IActionResult> RedirectToLongUrl(string path)
        {
            var urlMapping = await MappingsService.GetMappingByPath(path);

            if (urlMapping == null)
                return NotFound();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            // Google ip for testing
            ipAddress = "2a00:1450:4016:80a::2003";
            var ipData = await IpLookupService.GetDataAsync(ipAddress);

            // TODO: Fetch location
            await RedirectLogService.LogRedirectAsync(urlMapping, ipAddress, userAgent, ipData?.Lat, ipData?.Lon);

            return Redirect(urlMapping.LongUrl);
        }
    }
}
