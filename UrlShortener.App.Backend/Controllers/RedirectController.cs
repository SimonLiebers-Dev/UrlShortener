using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend.Controllers
{
    [ApiController]
    [Route("/")]
    [AllowAnonymous]
    internal class RedirectController(IMappingsService MappingsService, IRedirectLogService RedirectLogService, IUserAgentService UserAgentService) : ControllerBase
    {
        [HttpGet("{path}")]
        public async Task<IActionResult> RedirectToLongUrl(string path)
        {
            var urlMapping = await MappingsService.GetMappingByPath(path);

            if (urlMapping == null)
                return NotFound();

            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            // Fallback to default remote ip
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
            var userAgentData = await UserAgentService.GetUserAgentAsync(userAgent);

            await RedirectLogService.LogRedirectAsync(urlMapping, userAgentData, ipAddress, userAgent);

            return Redirect(urlMapping.LongUrl);
        }
    }
}
